using Doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Doan.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DoanContext _context;

        public DoctorController(DoanContext context)
        {
            _context = context;
        }

        // Trang danh sách bác sĩ
        public IActionResult Index()
        {
            // Lấy toàn bộ bác sĩ từ bảng tb_doctor
            var list =  _context.TbDoctors.ToList();
            return View(list);
        }

        // Trang chi tiết bác sĩ
        [Route("/Doctor/{alias}-{id}.html")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbDoctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.TbDoctors
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.DoctorId == id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }
    }
}
