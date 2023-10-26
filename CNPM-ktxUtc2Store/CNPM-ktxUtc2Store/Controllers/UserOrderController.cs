using CNPM_ktxUtc2Store.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly IUserOrderService _userOrderService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderController(IUserOrderService userOrderService, ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userOrderService = userOrderService;
            _httpContextAccessor = httpContextAccessor;
            _usermanagement = userManager;

        }
        public async Task<IActionResult> Userorder()
        {
            var orders = await _userOrderService.UserOrders();
            return View(orders);
        }


        //public async Task<IActionResult> dathang(int productId, int quantity)
        //{
        //    using var transaction = _context.Database.BeginTransaction();
        //    var userid = GetUserId();
        //    try
        //    {
        //        if (string.IsNullOrEmpty(userid))
        //        {
        //            return Redirect("/Identity/Account/Login");
        //        }
        //        var dathang = await GetDatHang(userid);
        //        if (dathang is null)
        //        {
        //            dathang = new order
        //            {
        //                userId = userid,
        //                createDate = DateTime.UtcNow,
        //                orderStatusId = 1
        //            };
        //            _context.orders.Add(dathang);
        //        }
        //        _context.SaveChanges();
        //        var CTDH = _context.orderDetails.FirstOrDefault(x => x.orderId == dathang.Id && x.productId == productId);
        //        if (CTDH is not null)
        //        {
        //            CTDH.quantity = CTDH.quantity + quantity;
        //        }
        //        else
        //        {
        //            var product = _context.products.Find(productId);
        //            CTDH = new orderDetail
        //            {
        //                productId = productId,
        //                orderId = dathang.Id,
        //                quantity = model.,
        //                unitPrice = product.price
        //            };
        //            _context.orderDetails.Add(CTDH);
        //        }
        //        _context.SaveChanges();
        //        transaction.Commit();
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return RedirectToAction("Userorder", "UserOrder");
        //}
        public async Task<order> GetDatHang(string userId)
        {
            var dathang = _context.orders.FirstOrDefault(x => x.userId == userId);
            return dathang;
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }
        public async Task<int> GetCTDHCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }

            var data = await (from order in _context.orders
                              join orderDetail in _context.orderDetails
                              on order.Id equals orderDetail.orderId
                              select new { orderDetail.Id }
                              ).ToListAsync();
            return data.Count;
        }


    }
}
