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

        public async Task<string> AskAsync(string question, CancellationToken cancellationToken = default)
        {
            var normalizedQuestion = Normalize(question);

            var services = await _context.TbServices
                .Include(s => s.Doctor)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var doctors = await _context.TbDoctors
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // ===== MATCH DỊCH VỤ =====
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

            // ===== MATCH BÁC SĨ =====
            TbDoctor? matchedDoctor = null;
            if (matchedService == null)
            {
                matchedDoctor = doctors.FirstOrDefault(d =>
                    normalizedQuestion.Contains(Normalize(d.FullName))
                );
            }

            var context = new StringBuilder();
            context.AppendLine("WEBSITE: Hệ thống đặt lịch khám bệnh");

            if (matchedService != null)
            {
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
            }
            else if (matchedDoctor != null)
            {
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
