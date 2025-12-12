using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doan.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Doan.Controllers
{
    public class CartController : Controller
    {
        private readonly DoanContext _context;

        public CartController(DoanContext context)
        {
            _context = context;
        }

        #region Giỏ Hàng

        // Hiển thị giỏ hàng của người dùng
        public async Task<IActionResult> ViewCart()
        {
            var accountId = GetAccountIdFromSession();
            if (accountId == 0)
                return RedirectToAction("Login", "Account");

            var cart = await _context.TbCarts
                .Where(c => c.AccountId == accountId && c.IsActive)
                .Include(c => c.TbCartItems)
                .ThenInclude(ci => ci.Service)
                .FirstOrDefaultAsync();

            if (cart == null)
                cart = new TbCart { TbCartItems = new List<TbCartItem>() };
            else if (cart.TbCartItems == null)
                cart.TbCartItems = new List<TbCartItem>();

            return View(cart);
        }

        #endregion

        #region Thêm vào giỏ

        [HttpPost]
        public async Task<IActionResult> AddToCart(int serviceId, int quantity, string doctorName)
        {
            var accountId = GetAccountIdFromSession();
            if (accountId == 0)
                return RedirectToAction("Login", "Account");

            var cart = await _context.TbCarts
                .FirstOrDefaultAsync(c => c.AccountId == accountId && c.IsActive);

            if (cart == null)
            {
                cart = new TbCart
                {
                    AccountId = accountId,
                    IsActive = true,
                    CreatedDate = System.DateTime.Now
                };
                _context.TbCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = await _context.TbCartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId && ci.ServiceId == serviceId);

            if (cartItem == null)
            {
                cartItem = new TbCartItem
                {
                    CartId = cart.CartId,
                    ServiceId = serviceId,
                    Quantity = quantity,
                    Price = GetServicePrice(serviceId),
                    DoctorName = doctorName
                };
                _context.TbCartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ViewCart");
        }

        #endregion

        #region Checkout

        // Hiển thị trang checkout
        public async Task<IActionResult> Checkout()
        {
            var accountId = GetAccountIdFromSession();
            if (accountId == 0)
                return RedirectToAction("Login", "Account");

            var cart = await _context.TbCarts
                .Where(c => c.AccountId == accountId && c.IsActive)
                .Include(c => c.TbCartItems)
                .ThenInclude(ci => ci.Service)
                .FirstOrDefaultAsync();

            if (cart == null || cart.TbCartItems == null || !cart.TbCartItems.Any())
                return RedirectToAction("ViewCart");

            return View("Checkout", cart);
        }

        // Xác nhận thanh toán và lưu đơn hàng vào SQL
        [HttpPost]
        public async Task<IActionResult> CheckoutConfirm()
        {
            var accountId = GetAccountIdFromSession();
            if (accountId == 0)
                return RedirectToAction("Login", "Account");

            var cart = await _context.TbCarts
                .Where(c => c.AccountId == accountId && c.IsActive)
                .Include(c => c.TbCartItems)
                .ThenInclude(ci => ci.Service)
                .FirstOrDefaultAsync();

            if (cart == null || cart.TbCartItems == null || !cart.TbCartItems.Any())
                return RedirectToAction("ViewCart");

            var totalAmount = cart.TbCartItems.Sum(ci => ci.Quantity * ci.Price);

            // 1. Tạo Order và save ngay để có OrderId
            var order = new TbOrder
            {
                AccountId = accountId,
                TotalAmount = totalAmount,
                PaymentStatus = false, // có thể đổi true nếu thanh toán online thành công
                OrderStatus = "Pending",
                CreatedDate = System.DateTime.Now
            };
            _context.TbOrders.Add(order);
            await _context.SaveChangesAsync(); // OrderId sẽ được sinh ở đây

            // 2. Tạo OrderItems từ giỏ hàng
            foreach (var item in cart.TbCartItems)
            {
                var orderItem = new TbOrderItem
                {
                    OrderId = order.OrderId,
                    ServiceId = item.ServiceId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    CategoryName = item.CategoryName ?? "",
                    DoctorName = item.DoctorName ?? ""
                };
                _context.TbOrderItems.Add(orderItem);
            }

            // 3. Đánh dấu giỏ hàng không hoạt động
            cart.IsActive = false;

            // 4. Lưu tất cả thay đổi
            await _context.SaveChangesAsync();

            // 5. Thông báo thành công
            TempData["PaymentMessage"] = "Bạn đã thanh toán thành công!";

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        #endregion

        #region Order Confirmation

        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _context.TbOrders
                .Include(o => o.TbOrderItems)
                .ThenInclude(oi => oi.Service)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return NotFound();

            return View(order);
        }

        #endregion

        #region Hỗ Trợ

        private int GetAccountIdFromSession()
        {
            return HttpContext.Session.GetInt32("AccountId") ?? 0;
        }

        private int GetServicePrice(int serviceId)
        {
            var service = _context.TbServices.FirstOrDefault(s => s.ServiceId == serviceId);
            return service?.Price ?? 0;
        }

        #endregion
    }
}
