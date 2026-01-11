using Doan.Models;
using Doan.service.Llm;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Doan.service
{
    public class RagPipeline
    {
        private readonly ILlmChatProvider _llm;
        private readonly DoanContext _context;

        public RagPipeline(ILlmChatProvider llm, DoanContext context)
        {
            _llm = llm;
            _context = context;
        }

        // Chuẩn hóa chuỗi – bỏ dấu tiếng Việt
        private static string Normalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.ToLowerInvariant();
            input = input.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();
            foreach (var c in input)
            {
                if (Char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            input = sb.ToString().Normalize(NormalizationForm.FormC);
            input = Regex.Replace(input, @"[^\p{L}\p{N}\s]", "");
            input = Regex.Replace(input, @"\s+", " ").Trim();

            return input;
        }

        // ===== ADDED: nhận biết câu hỏi đang muốn xem chi tiết bác sĩ =====
        private static bool IsAskingDoctorDetail(string normalizedQuestion)
        {
            if (string.IsNullOrWhiteSpace(normalizedQuestion)) return false;

            // Bắt rộng để không bị rớt case: "cho toi them thong tin", "xin them", "noi ro", ...
            return normalizedQuestion.Contains("bac si") &&
                   (normalizedQuestion.Contains("thong tin")
                    || normalizedQuestion.Contains("tieu su")
                    || normalizedQuestion.Contains("chi tiet")
                    || normalizedQuestion.Contains("kinh nghiem")
                    || normalizedQuestion.Contains("gioi thieu")
                    || normalizedQuestion.Contains("noi ro")
                    || normalizedQuestion.Contains("xin them")
                    || normalizedQuestion.Contains("them thong tin")
                    || normalizedQuestion.Contains("profile"));
        }

        // ===== ADDED: match mềm tên bác sĩ theo token =====
        private static int ScoreNameMatch(string normalizedQuestion, string normalizedFullName)
        {
            if (string.IsNullOrWhiteSpace(normalizedQuestion) || string.IsNullOrWhiteSpace(normalizedFullName))
                return 0;

            // Khớp nguyên cụm họ tên -> ưu tiên tuyệt đối
            if (normalizedQuestion.Contains(normalizedFullName))
                return 999;

            var tokens = normalizedFullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int score = 0;
            foreach (var t in tokens)
            {
                if (t.Length < 2) continue;
                if (normalizedQuestion.Contains(t))
                    score++;
            }

            return score;
        }

        public async Task<string> AskAsync(string question, CancellationToken cancellationToken = default)
        {
            var normalizedQuestion = Normalize(question);

            // ===== 1. LOAD DATA GỐC (GIỮ NGUYÊN) =====
            var services = await _context.TbServices
                .Include(s => s.Doctor)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var doctors = await _context.TbDoctors
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // ===== 2. MATCH DỊCH VỤ (GIỮ NGUYÊN) =====
            TbService? matchedService = services
                .Select(s =>
                {
                    var title = Normalize(s.Title).Replace("dich vu", "").Trim();
                    var keywords = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var score = keywords.Count(k => normalizedQuestion.Contains(k));
                    return new { Service = s, Score = score };
                })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault(x => x.Score > 0)
                ?.Service;

            // ===== 3. MATCH BÁC SĨ (SỬA CHO CHẮC ĂN) =====
            // Luôn tìm candidate bác sĩ, dù có match dịch vụ hay không
            var doctorCandidate = doctors
                .Select(d =>
                {
                    var fullName = Normalize(d.FullName);
                    var score = ScoreNameMatch(normalizedQuestion, fullName);
                    return new { Doctor = d, Score = score };
                })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            TbDoctor? matchedDoctor = null;

            // Nếu câu hỏi có ý hỏi chi tiết bác sĩ HOẶC câu hỏi đang chứa tên bác sĩ (khớp mạnh)
            bool wantDoctor =
                IsAskingDoctorDetail(normalizedQuestion)
                || (doctorCandidate != null && doctorCandidate.Score == 999); // có đầy đủ họ tên trong câu

            // Ngưỡng khớp: >=2 token (vd "nguyen"+"anh", "tran"+"hai") hoặc khớp full
            if (doctorCandidate != null && (doctorCandidate.Score == 999 || doctorCandidate.Score >= 2))
            {
                // Nếu user đang muốn thông tin bác sĩ -> ưu tiên bác sĩ luôn
                if (wantDoctor)
                    matchedDoctor = doctorCandidate.Doctor;
                else
                {
                    // Giữ tinh thần code cũ: chỉ match bác sĩ khi không match dịch vụ
                    if (matchedService == null)
                        matchedDoctor = doctorCandidate.Doctor;
                }
            }

            // ===== 4. BUILD CONTEXT (GIỮ ĐẦY ĐỦ DỊCH VỤ, ƯU TIÊN BÁC SĨ KHI CẦN) =====
            var context = new StringBuilder();
            context.AppendLine("WEBSITE: Hệ thống đặt lịch khám bệnh");

            // Ưu tiên trả hồ sơ bác sĩ nếu user đang hỏi về bác sĩ
            if (matchedDoctor != null && (IsAskingDoctorDetail(normalizedQuestion) || (doctorCandidate?.Score == 999)))
            {
                string experience =
                    !string.IsNullOrWhiteSpace(matchedDoctor.ExperienceYears)
                        ? matchedDoctor.ExperienceYears
                        : "Chưa rõ";

                context.AppendLine($@"
BÁC SĨ:
- Họ tên: {matchedDoctor.FullName}
- Giới tính: {matchedDoctor.Gender ?? "Đang cập nhật"}
- Ngày sinh: {(matchedDoctor.DateOfBirth.HasValue ? matchedDoctor.DateOfBirth.Value.ToString("dd/MM/yyyy") : "Đang cập nhật")}
- SĐT: {matchedDoctor.Phone ?? "Đang cập nhật"}
- Email: {matchedDoctor.Email ?? "Đang cập nhật"}
- Địa chỉ: {matchedDoctor.Address ?? "Đang cập nhật"}
- Chức vụ: {matchedDoctor.Position ?? "Đang cập nhật"}
- Trình độ: {matchedDoctor.Qualification ?? "Đang cập nhật"}
- Kinh nghiệm: {experience}
- Giới thiệu: {matchedDoctor.Description ?? "Đang cập nhật"}
");
            }
            else if (matchedService != null)
            {
                // GIỮ NGUYÊN NHÁNH DỊCH VỤ
                string experience =
                    !string.IsNullOrWhiteSpace(matchedService.Doctor?.ExperienceYears)
                        ? matchedService.Doctor.ExperienceYears
                        : "Chưa rõ";

                context.AppendLine($@"
DỊCH VỤ:
- Tên: {matchedService.Title}
- Mô tả: {matchedService.Detail}
- Giá: {(matchedService.Price.HasValue ? matchedService.Price.Value.ToString("N0") + " VNĐ" : "Liên hệ")}

BÁC SĨ PHỤ TRÁCH:
- Họ tên: {matchedService.Doctor?.FullName ?? "Đang cập nhật"}
- Chức vụ: {matchedService.Doctor?.Position ?? "Đang cập nhật"}
- Kinh nghiệm: {experience}
");

                // Nếu user hỏi kiểu “bác sĩ phụ trách dịch vụ X là ai” thì chỉ cần thế là đủ.
                // Nếu bạn muốn khi hỏi dịch vụ mà có từ “chi tiết bác sĩ” thì cũng show thêm profile bác sĩ phụ trách:
                if (IsAskingDoctorDetail(normalizedQuestion) && matchedService.Doctor != null)
                {
                    var d = matchedService.Doctor;
                    var exp = !string.IsNullOrWhiteSpace(d.ExperienceYears) ? d.ExperienceYears : "Chưa rõ";

                    context.AppendLine($@"
TIỂU SỬ BÁC SĨ PHỤ TRÁCH:
- Họ tên: {d.FullName}
- Giới tính: {d.Gender ?? "Đang cập nhật"}
- Ngày sinh: {(d.DateOfBirth.HasValue ? d.DateOfBirth.Value.ToString("dd/MM/yyyy") : "Đang cập nhật")}
- SĐT: {d.Phone ?? "Đang cập nhật"}
- Email: {d.Email ?? "Đang cập nhật"}
- Địa chỉ: {d.Address ?? "Đang cập nhật"}
- Chức vụ: {d.Position ?? "Đang cập nhật"}
- Trình độ: {d.Qualification ?? "Đang cập nhật"}
- Kinh nghiệm: {exp}
- Giới thiệu: {d.Description ?? "Đang cập nhật"}
");
                }
            }
            else if (matchedDoctor != null)
            {
                // GIỮ NGUYÊN NHÁNH BÁC SĨ CŨ (khi không match dịch vụ)
                string experience =
                    !string.IsNullOrWhiteSpace(matchedDoctor.ExperienceYears)
                        ? matchedDoctor.ExperienceYears
                        : "Chưa rõ";

                context.AppendLine($@"
BÁC SĨ:
- Họ tên: {matchedDoctor.FullName}
- Chức vụ: {matchedDoctor.Position}
- Trình độ: {matchedDoctor.Qualification}
- Kinh nghiệm: {experience}
- Giới thiệu: {matchedDoctor.Description}
");
            }
            else
            {
                // GIỮ NGUYÊN: list tất cả dịch vụ khi không match
                context.AppendLine("Không tìm thấy dịch vụ phù hợp. Các dịch vụ hiện có:");
                foreach (var s in services)
                    context.AppendLine("- " + s.Title);
            }

            var systemPrompt = """
Bạn là trợ lý AI cho website đặt lịch khám bệnh.
Chỉ trả lời dựa trên CONTEXT.
Không bịa thông tin.
""";

            var userPrompt = $"""
CONTEXT:
{context}

CÂU HỎI:
{question}
""";

            return await _llm.AskAsync(systemPrompt, userPrompt, cancellationToken);
        }
    }
}
