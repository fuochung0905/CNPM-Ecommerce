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
      
        public async Task<shoppingCart> GetUserCart()
        {
            var userId = GetUserId();

            if (userId == null)
                throw new Exception("Invalid user");
            var shoppingcart = await _context.shoppingCarts
                .Include(a => a.cartDetails)
                .ThenInclude(a => a.product)
                .ThenInclude(a => a.category)
                .Where(a => a.applicationUserId == userId).FirstOrDefaultAsync();
            return shoppingcart;


        }
        public async Task<IActionResult> Userorder()
         {
            var cart = await GetUserCart();
            cartToOrder a = new cartToOrder();
            a.shoppingCarts = cart;

            return View(a);
        }
    
        [HttpPost]
        public async Task<IActionResult> Userorder(cartToOrder cartTo)
        {
            var userid = GetUserId();
            var userAdress= await _context.userAdresses.Include(x=>x.adress).Include(x=>x.applicationUser)
                .Where(x=>x.applicationUserId==userid).Where(x=>x.isDefine==true).ToListAsync();
            foreach( var item in userAdress)
            {
                if(item != null)
                {
                    var cartDetail = await _context.cartDetails.FindAsync(cartTo.Id);
                    if (cartDetail != null)
                    {
                        using var transaction = _context.Database.BeginTransaction();
                        try
                        {
                            var applicationUser = _context.applicationUsers.Find(userid);
                            //var dathang =  GetDatHang(userid);

                            var dathang = new order
                            {
                                applicationUserId = userid,
                                createDate = DateTime.UtcNow,
                                orderStatusId = 1
                            };
                            _context.orders.Add(dathang);

                            _context.SaveChanges();
                            var CTDH = _context.orderDetails.FirstOrDefault(x => x.orderId == dathang.Id);
                            var product = _context.products.Find(cartDetail.productId);
                            CTDH = new orderDetail
                            {
                                productId = cartDetail.productId,
                                orderId = dathang.Id,
                                quantity = cartDetail.quantity,
                                size = cartDetail.size,
                                color = cartDetail.color,
                                unitPrice = product.price
                            };
                            _context.orderDetails.Add(CTDH);
                            _context.cartDetails.Remove(cartDetail);
                            product.qty_inStock = product.qty_inStock - cartDetail.quantity;
                            _context.products.Update(product);
                            _context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                        }

                    }
                    return Content("Đặt hàng thành công");
                }
            }
            return Content("Cần chọn địa chỉ người dụng");

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
