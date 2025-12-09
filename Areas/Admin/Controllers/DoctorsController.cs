using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Doan.Models;

namespace Doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoctorsController : Controller
    {
        private readonly DoanContext _context;

        public DoctorsController(DoanContext context)
        {
            _context = context;
        }

        // GET: Admin/Doctors
        public async Task<IActionResult> Index()
        {
            var doanContext = _context.TbDoctors.Include(t => t.Account).Include(t => t.Category);
            return View(await doanContext.ToListAsync());
        }

        // GET: Admin/Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDoctor = await _context.TbDoctors
                .Include(t => t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (tbDoctor == null)
            {
                return NotFound();
            }

            return View(tbDoctor);
        }

        // GET: Admin/Doctors/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.TbAccounts, "AccountId", "AccountId");
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Admin/Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorId,FullName,Gender,DateOfBirth,Phone,Email,Address,CategoryId,Position,Qualification,ExperienceYears,Description,Image,AccountId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive,IsNew")] TbDoctor tbDoctor)
        {
            if (ModelState.IsValid)
            {

                _context.Add(tbDoctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.TbAccounts, "AccountId", "AccountId", tbDoctor.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId", tbDoctor.CategoryId);
            return View(tbDoctor);
        }

        // GET: Admin/Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDoctor = await _context.TbDoctors.FindAsync(id);
            if (tbDoctor == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.TbAccounts, "AccountId", "AccountId", tbDoctor.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId", tbDoctor.CategoryId);
            return View(tbDoctor);
        }

        // POST: Admin/Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorId,FullName,Gender,DateOfBirth,Phone,Email,Address,CategoryId,Position,Qualification,ExperienceYears,Description,Image,AccountId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive,IsNew")] TbDoctor tbDoctor)
        {
            if (id != tbDoctor.DoctorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbDoctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbDoctorExists(tbDoctor.DoctorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.TbAccounts, "AccountId", "AccountId", tbDoctor.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId", tbDoctor.CategoryId);
            return View(tbDoctor);
        }

        // GET: Admin/Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDoctor = await _context.TbDoctors
                .Include(t => t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (tbDoctor == null)
            {
                return NotFound();
            }

            return View(tbDoctor);
        }

        // POST: Admin/Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbDoctor = await _context.TbDoctors.FindAsync(id);
            if (tbDoctor != null)
            {
                _context.TbDoctors.Remove(tbDoctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbDoctorExists(int id)
        {
            return _context.TbDoctors.Any(e => e.DoctorId == id);
        }
    }
}
