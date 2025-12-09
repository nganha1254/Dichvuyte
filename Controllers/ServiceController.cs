using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doan.Models;
using System.Threading.Tasks;

namespace Doan.Controllers
{
    public class ServiceController : Controller
    {
        private readonly DoanContext _context;

        public ServiceController(DoanContext context)
        {
            _context = context;
        }

        // GET: Service/Index
        public async Task<IActionResult> Index()
        {
            // Lấy tất cả các dịch vụ từ bảng TbServices, bao gồm Category và Doctor
            var services = await _context.TbServices
                .Include(s => s.Category)  // Bao gồm thông tin về Category
                .Include(s => s.Doctor)    // Bao gồm thông tin về Doctor (bác sĩ phụ trách)
                .Where(s => s.IsActive)    // Chỉ lấy dịch vụ đang hoạt động
                .ToListAsync();            // Lấy tất cả dịch vụ vào danh sách

            return View(services); // Trả về view và truyền danh sách dịch vụ vào
        }

        // GET: Service/Details/5
        // Hiển thị chi tiết dịch vụ cụ thể theo ServiceId
        public async Task<IActionResult> Details(int id)
        {
            // Lấy dịch vụ cụ thể theo ServiceId, bao gồm thông tin về Category và Doctor
            var service = await _context.TbServices
                .Include(s => s.Category)  // Bao gồm thông tin về Category
                .Include(s => s.Doctor)    // Bao gồm thông tin về Doctor
                .FirstOrDefaultAsync(s => s.ServiceId == id);  // Lấy dịch vụ theo ID

            if (service == null)
            {
                return NotFound();
            }

            return View(service);  // Trả về view chi tiết và truyền đối tượng service vào
        }
    }
}
