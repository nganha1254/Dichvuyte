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
    [Route("bac-si")]
    public IActionResult Doctors()
    {
        return View("~/Views/Doctor/Index.cshtml");
    }
    [Route("dich-vu")]
    public IActionResult Services()
    {
        return View();
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
