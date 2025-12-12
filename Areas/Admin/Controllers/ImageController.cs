using Doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ImageController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DoanContext _context;
        public ImageController(DoanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(string path = "")
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file, string folder)
        {
            folder ??= "";

            string uploadPath = Path.Combine(_env.WebRootPath, "uploads", folder);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            if (file != null)
            {
                string filePath = Path.Combine(uploadPath, file.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
            }

            return RedirectToAction("Index", new { path = folder });
        }

        [HttpGet]
        public IActionResult GetImages(string path = "")
        {
            string root = Path.Combine(_env.WebRootPath, "uploads");
            string current = string.IsNullOrEmpty(path) ? root : Path.Combine(root, path);

            // Thư mục
            var folders = Directory.GetDirectories(current)
                .Select(d => new
                {
                    name = Path.GetFileName(d),
                    path = Path.GetRelativePath(root, d).Replace("\\", "/")
                });

            // File
            var files = Directory.GetFiles(current)
                .Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png"))
                .Select(f => new
                {
                    name = Path.GetFileName(f),
                    url = "/uploads/" + Path.GetRelativePath(root, f).Replace("\\", "/"),
                    relativePath = Path.GetRelativePath(root, f).Replace("\\", "/")
                });

            return Json(new
            {
                current = Path.GetRelativePath(root, current).Replace("\\", "/"),
                folders,
                files
            });
        }

        [HttpPost]
        public IActionResult Delete([FromBody] dynamic data)
        {
            string path = data.path;
            string full = Path.Combine(_env.WebRootPath, "uploads", path);

            if (System.IO.File.Exists(full))
                System.IO.File.Delete(full);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Rename([FromBody] dynamic data)
        {
            string path = data.path;
            string newName = data.newName;

            string full = Path.Combine(_env.WebRootPath, "uploads", path);

            string folder = Path.GetDirectoryName(full);
            string newPath = Path.Combine(folder!, newName);

            System.IO.File.Move(full, newPath);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult MoveFile(string source, string destination)
        {
            string root = Path.Combine(_env.WebRootPath, "uploads");

            string fullSource = Path.Combine(root, source);
            string fullDestination = Path.Combine(root, destination);

            if (!System.IO.File.Exists(fullSource))
                return Json(new { success = false, message = "File không tồn tại." });

            if (!Directory.Exists(fullDestination))
                Directory.CreateDirectory(fullDestination);

            string fileName = Path.GetFileName(fullSource);
            string newPath = Path.Combine(fullDestination, fileName);

            System.IO.File.Move(fullSource, newPath, true);

            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TbDoctor model, IFormFile ImageUpload)
        {
            if (id != model.DoctorId) return NotFound();

            var doctor = _context.TbDoctors.Find(id);
            if (doctor == null) return NotFound();

            if (ImageUpload != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads/doctors");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(ImageUpload.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(stream);
                }

                doctor.Image = fileName;
            }

            doctor.FullName = model.FullName;
            doctor.Gender = model.Gender;
            doctor.DateOfBirth = model.DateOfBirth;
            doctor.Phone = model.Phone;
            doctor.Email = model.Email;
            doctor.Address = model.Address;
            doctor.CategoryId = model.CategoryId;
            doctor.Position = model.Position;
            doctor.Qualification = model.Qualification;
            doctor.ExperienceYears = model.ExperienceYears;
            doctor.Description = model.Description;
            doctor.IsActive = model.IsActive;
            doctor.IsNew = model.IsNew;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
