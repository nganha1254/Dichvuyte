using Doan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class AccountController : Controller
{
    private readonly DoanContext _context;

    public AccountController(DoanContext context)
    {
        _context = context;
    }

    // Hiển thị trang đăng nhập
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    // Xử lý đăng nhập
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra thông tin người dùng trong cơ sở dữ liệu
            var account = _context.TbAccounts
                .FirstOrDefault(a => a.Username == model.Username && a.Password == model.Password);

            if (account != null)
            {
                // Lưu AccountId vào session khi đăng nhập thành công
                HttpContext.Session.SetInt32("AccountId", account.AccountId);

                // Điều hướng đến trang giỏ hàng hoặc trang khác
                return RedirectToAction("ViewCart", "Cart");
            }
            else
            {
                // Nếu thông tin đăng nhập không hợp lệ, sử dụng TempData để hiển thị thông báo lỗi
                TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không chính xác!";
            }
        }
        else
        {
            // Nếu model không hợp lệ, thông báo lỗi sẽ hiển thị cho người dùng
            TempData["ErrorMessage"] = "Vui lòng điền đầy đủ thông tin!";
        }

        // Trả về view và giữ lại thông tin người dùng nhập vào (model)
        return View(model);
    }
}
