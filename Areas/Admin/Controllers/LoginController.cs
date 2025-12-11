using Doan.Models;
using Doan.Utilities;
using Doan.Models;
using Doan.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doan.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly DoanContext _context;

        public LoginController(DoanContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Message = Function._Message;
            return View();
        }

        [HttpPost]
        public IActionResult Index(TbAccount account)
        {
            if (account == null)
            {
                return NotFound();
            }
            // Trim username and password to avoid whitespace issues
            string username = account.Username?.Trim();
            string passwordInput = account.Password?.Trim();

            // Check if the password in the database is hashed or plain text
            // If your database stores plain text passwords, use passwordInput directly
            // If your database stores hashed passwords, use HashMD5.GetMD5(passwordInput)
            string password = HashMD5.GetMD5(passwordInput);

            var check = _context.TbAccounts
                .FirstOrDefault(m => m.Username == username && m.Password == password && m.IsActive == true);

            if (check == null)
            {
                // Try plain text password if hash fails (for testing)
                check = _context.TbAccounts
                    .FirstOrDefault(m => m.Username == username && m.Password == passwordInput && m.IsActive == true);
            }

            if (check == null)
            {
                Function._Message = "Invalid Username or Password";
                return RedirectToAction("Index", "Login");
            }

            Function._Message = string.Empty;
            Function._AccountId = check.AccountId;
            Function._UserName = check.Username;

            return RedirectToAction("Index", "Home");
        }
    }
}
