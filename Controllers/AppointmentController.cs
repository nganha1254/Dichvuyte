using Doan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Doan.Controllers
{
    public class AppointmentController : Controller
    {
        public readonly DoanContext _context;
        public AppointmentController(DoanContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name, string phone, string email, string message, string CreatedDate, string CategoryName, string DoctorName)
        {
            try
            {
                TbAppointment appointment = new TbAppointment();
                appointment.FullName = name;
                appointment.Phone = phone;
                appointment.Email = email;
                appointment.Message = message;
                appointment.CreatedDate = DateTime.Parse(CreatedDate);
                appointment.CategoryName = CategoryName;
                appointment.DoctorName = DoctorName;

                _context.Add(appointment);
                await _context.SaveChangesAsync();

                return Json(new { status = true });
            }
            catch
            {
                return Json(new { status = false });
            }
        }
    }
}
