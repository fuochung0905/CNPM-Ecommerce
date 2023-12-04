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
        private readonly UserManager<applicationUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderController(IUserOrderService userOrderService, ApplicationDbContext context, UserManager<applicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userOrderService = userOrderService;
            _httpContextAccessor = httpContextAccessor;
            _usermanagement = userManager;

        }
      
  
            public async Task<IActionResult> Userorder()
        {
            var userid = GetUserId();
            if (string.IsNullOrWhiteSpace(userid))
            {
                throw new Exception("User is not logged-in");
            }

            var orders = await _context.orders
                .Include(x => x.status)
                .Include(x => x.orderDetails)
                .ThenInclude(x => x.product)
                .ThenInclude(x => x.category)
                .Where(a => a.applicationUser.Id == userid && a.IsDelete==false).ToListAsync();
            var list = new doneOrder();
           foreach (var order in orders)
            {
                list.orderList.Add(order);
            }

            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Userorder(doneOrder doneOrder)
        {
            var userid = GetUserId();
            var userAdress= await _context.userAdresses.Include(x=>x.adress).Include(x=>x.applicationUser)
                .Where(x=>x.applicationUserId==userid).Where(x=>x.isDefine==true).ToListAsync();
          foreach( var item in userAdress)
            {
                if(item != null)
                {
                    var detailorder = await _context.orderDetails.FindAsync(doneOrder.orderId);
                    var order = await _context.orders.FindAsync(doneOrder.orderId);
                    order.IsDelete = true;
                    _context.orders.Update(order);
                    var product = await _context.products.FindAsync(detailorder.productId);
                    product.qty_inStock = product.qty_inStock - detailorder.quantity;
                    _context.products.Update(product); 
                    _context.SaveChanges();
                    return Content("Đã đặt hàng");
                }
            }
            return Content("Cần chọn địa chỉ người dụng");

        }
        public IActionResult Complete()
        {
            return View();
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
            var dathang = _context.orders.FirstOrDefault(x => x.applicationUser.Id == userId);
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
        public double tongtien(doneOrder doneOrder)
        {
           var orderdetail= _context.orderDetails.Where(x=>x.orderId==doneOrder.orderId).FirstOrDefault();
            if (orderdetail!=null)
            {
                return orderdetail.quantity * orderdetail.unitPrice;
            }
            return 0;
        }


    }
}
