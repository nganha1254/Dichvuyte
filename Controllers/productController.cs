using Doan.Models;
using Microsoft.AspNetCore.Mvc;

public class productController : Controller
{
    private readonly DoanContext _context;

    public productController(DoanContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Lấy danh sách product đang active (IsActive = true)
        var services = _context.TbProducts
                               .Where(p => p.IsActive)
                               .ToList();

        return View(services);   // ✔ TRẢ MODEL ĐÚNG
    }
}
