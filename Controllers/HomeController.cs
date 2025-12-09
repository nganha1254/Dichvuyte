using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Doan.Models;
using Microsoft.EntityFrameworkCore;

namespace Doan.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DoanContext _context;

    public HomeController(ILogger<HomeController> logger, DoanContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }


    [Route("gioi-thieu")]
    public IActionResult About()
    {
        // alias và id bạn muốn redirect đến
        return RedirectToAction("Details", "About", new { alias = "gioi-thieu", id = 1 });
    }


    [Route("bac-si")]
    public IActionResult Doctors()
    {
        var list = _context.TbDoctors.ToList();
        return View("~/Views/Doctor/Index.cshtml", list);
    }



    [Route("dich-vu")]
    public IActionResult Services()
    {
        // Lấy tất cả dịch vụ từ cơ sở dữ liệu, bao gồm thông tin Category và Doctor
        var services = _context.TbServices
            .Include(s => s.Category)  // Bao gồm thông tin về Category
            .Include(s => s.Doctor)    // Bao gồm thông tin về Doctor (bác sĩ phụ trách)
            .Where(s => s.IsActive)    // Lọc chỉ các dịch vụ đang hoạt động
            .ToList();                 // Lấy tất cả dịch vụ vào danh sách

        // Trả về view từ đường dẫn Views/Home/Service/Index.cshtml
        return View("~/Views/Service/Index.cshtml", services);  // Trả về view và truyền danh sách dịch vụ vào
    }


    [Route("Contact")]
    public IActionResult Contact()
    {
        return View("~/Views/Contact/Index.cshtml");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
