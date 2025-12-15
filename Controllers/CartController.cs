using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doan.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Doan.Payments;
using Microsoft.AspNetCore.Authorization;

namespace Doan.Controllers
{
    public class CartController : Controller
    {
        private readonly DoanContext _context;
        //ngân hàng
        private readonly VNPayService _vnPayService;

        public CartController(DoanContext context, VNPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
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
                PaymentStatus = true, // có thể đổi true nếu thanh toán online thành công
                OrderStatus = "Paid",
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
        //ngân hàng
        #region VNPAY - Thanh toán ngân hàng

        [HttpPost]
        public async Task<IActionResult> PayByBank(string paymentMethod = "VNBANK")
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

            // 1) Tạo Order trước để có OrderId (giống mẫu)
            var order = new TbOrder
            {
                AccountId = accountId,
                TotalAmount = totalAmount,
                PaymentStatus = false,
                OrderStatus = "Processing",
                CreatedDate = DateTime.Now
            };

            _context.TbOrders.Add(order);
            await _context.SaveChangesAsync();

            // 2) Tạo URL thanh toán VNPay
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

            // orderCode: dùng chính OrderId để dễ map khi callback
            var orderCode = order.OrderId.ToString();
            var orderDescription = $"Thanh toán đơn hàng #{order.OrderId}";

            // returnUrl: nếu bạn đã set trong appsettings, _vnPayService sẽ dùng baseReturnUrl
            // nhưng để chắc ăn vẫn có thể truyền explicit:
            var returnUrl = $"{Request.Scheme}://{Request.Host}/Cart/VNPayReturn";

            var paymentUrl = _vnPayService.CreatePaymentUrl(
                orderId: order.OrderId,
                orderCode: orderCode,
                amount: totalAmount,
                orderDescription: orderDescription,
                customerName: "",
                customerEmail: "",
                customerPhone: "",
                ipAddress: ipAddress,
                returnUrl: returnUrl,
                paymentMethod: paymentMethod // "VNBANK" hoặc "VNPAYQR"
            );

            return Redirect(paymentUrl);
        }
        #region VNPAY - Callback

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VNPayReturn()
        {
            var queryParams = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());

            if (queryParams == null || queryParams.Count == 0)
                return RedirectToAction("ViewCart");

            var paymentResponse = _vnPayService.ProcessPaymentResponse(queryParams);

            if (!paymentResponse.Success)
            {
                TempData["PaymentMessage"] = $"Thanh toán thất bại (Code: {paymentResponse.ResponseCode}).";
                return RedirectToAction("ViewCart");
            }

            // vnp_TxnRef = orderCode (mình set bằng OrderId string)
            if (!int.TryParse(paymentResponse.OrderId, out var orderId))
            {
                TempData["PaymentMessage"] = "Không xác định được đơn hàng từ VNPay.";
                return RedirectToAction("ViewCart");
            }

            var order = await _context.TbOrders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                TempData["PaymentMessage"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("ViewCart");
            }

            // Nếu đã paid rồi thì tránh callback lặp
            if (!order.PaymentStatus)
            {
                order.PaymentStatus = true;
                order.OrderStatus = "Paid";

                // Tạo OrderItems từ giỏ hàng active hiện tại
                var cart = await _context.TbCarts
                    .Include(c => c.TbCartItems)
                    .FirstOrDefaultAsync(c => c.AccountId == order.AccountId && c.IsActive);

                if (cart != null && cart.TbCartItems != null && cart.TbCartItems.Any())
                {
                    foreach (var item in cart.TbCartItems)
                    {
                        _context.TbOrderItems.Add(new TbOrderItem
                        {
                            OrderId = order.OrderId,
                            ServiceId = item.ServiceId,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            CategoryName = item.CategoryName ?? "",
                            DoctorName = item.DoctorName ?? ""
                        });
                    }

                    cart.IsActive = false;
                }

                await _context.SaveChangesAsync();
            }

            TempData["PaymentMessage"] = "Thanh toán ngân hàng thành công!";
            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        #endregion

        #endregion

        #endregion

    }
}