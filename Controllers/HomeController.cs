using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Doan.Models;

namespace Doan.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [Route("lien-he")]
    [Route("bac-sy")]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    [Route("lien-he")]
    public IActionResult Contact(string name, string email, string subject, string message)
    {
        TempData["Success"] = "Gửi liên hệ thành công!";
        return RedirectToAction("Contact");
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
