using Doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doan.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DoanContext _context;
        public DoctorController(DoanContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("/Doctor/{alias}-{id}.html")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbDoctors == null)
            {
                return NotFound();
            }
            var Doctor = await _context.TbDoctors
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (Doctor == null)
            {
                return NotFound();
            }
            ViewBag.doctorDetails = _context.TbDoctors.Where(i => i.DoctorId == id).ToList();
            return View(Doctor);
        }
    }
}
