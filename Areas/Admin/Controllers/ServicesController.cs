using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Doan.Models;

namespace Doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicesController : Controller
    {
        private readonly DoanContext _context;

        public ServicesController(DoanContext context)
        {
            _context = context;
        }

        // GET: Admin/Services
        public async Task<IActionResult> Index()
        {
            var services = _context.TbServices
                .Include(s => s.Doctor)
                .Include(s => s.Category);

            return View(await services.ToListAsync());
        }

        // GET: Admin/Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _context.TbServices
                .Include(s => s.Doctor)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.ServiceId == id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        // GET: Admin/Services/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.TbDoctors, "DoctorId", "FullName");
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "Title");
            return View();
        }

        // POST: Admin/Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Alias,Icon,Image,ShortDescription,Detail,DoctorId,CategoryId,Position,SeoTitle,SeoDescription,SeoKeywords,IsNew,IsFeatured,IsActive,Price,PriceSale")]
            TbService service)
        {
            if (ModelState.IsValid)
            {
                service.Alias = Utilities.Function.TitleSlugGenerrationAlias(service.Title);
                service.CreatedDate = DateTime.Now;

                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DoctorId"] = new SelectList(_context.TbDoctors, "DoctorId", "FullName", service.DoctorId);
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "Title", service.CategoryId);
            return View(service);
        }

        // GET: Admin/Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _context.TbServices.FindAsync(id);
            if (service == null)
                return NotFound();

            ViewData["DoctorId"] = new SelectList(_context.TbDoctors, "DoctorId", "FullName", service.DoctorId);
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "Title", service.CategoryId);
            return View(service);
        }

        // POST: Admin/Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ServiceId,Title,Alias,Icon,Image,ShortDescription,Detail,DoctorId,CategoryId,Position,SeoTitle,SeoDescription,SeoKeywords,IsNew,IsFeatured,IsActive,Price,PriceSale,CreatedDate,CreatedBy")]
            TbService service)
        {
            if (id != service.ServiceId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    service.ModifiedDate = DateTime.Now;
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbServiceExists(service.ServiceId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DoctorId"] = new SelectList(_context.TbDoctors, "DoctorId", "FullName", service.DoctorId);
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "Title", service.CategoryId);
            return View(service);
        }

        // GET: Admin/Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _context.TbServices
                .Include(s => s.Doctor)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.ServiceId == id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        // POST: Admin/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.TbServices.FindAsync(id);
            if (service != null)
                _context.TbServices.Remove(service);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbServiceExists(int id)
        {
            return _context.TbServices.Any(e => e.ServiceId == id);
        }
    }
}
