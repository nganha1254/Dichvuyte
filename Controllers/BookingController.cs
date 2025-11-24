using Microsoft.AspNetCore.Mvc;

namespace Doan.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index(string type)
        {
            ViewBag.Type = type;
            return View();
        }

        [HttpPost]
        public IActionResult Submit(string type, string HoTen, string DienThoai, string Ngay, string GhiChu)
        {
            TempData["msg"] = "Đặt lịch thành công!";
            return RedirectToAction("Index", new { type = type });
        }
    }
}
