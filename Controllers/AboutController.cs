using Doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doan.Controllers
{
    public class AboutController : Controller
    {
        private readonly DoanContext _context;
        public AboutController(DoanContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("/About/{alias}-{id}.html")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbAbouts == null)
            {
                return NotFound();
            }
            var About = await _context.TbAbouts.FirstOrDefaultAsync(m => m.AboutId == id);
            if (About == null)
            {
                return NotFound();
            }
            ViewBag.aboutDetails = _context.TbAboutDetails.Where(i => i.AboutId == id).ToList();
            return View(About);
        }
    }
}
