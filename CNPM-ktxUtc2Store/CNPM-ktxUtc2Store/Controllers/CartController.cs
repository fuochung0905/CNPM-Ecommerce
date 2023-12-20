using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<applicationUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ICartService cartService, ApplicationDbContext context, UserManager<applicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _context = context;
            _usermanagement = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> AddItem(int productId,string color,string size, int quantity=1,int redirect=0)
        {
            var cartCount = await _cartService.AddItem(productId, quantity,size,color);
            if(redirect == 0) { 
                return Ok(cartCount);
            }
            return RedirectToAction("GetUserCart");
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }

        public async Task<IActionResult> Remove(int productId)
        {
            var cartCount= await _cartService.RemoveItem(productId);

            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart= await _cartService.GetUserCart();
            cartToOrder a = new cartToOrder();
            a.shoppingCarts = cart;
            return View(a);
        }
        [HttpPost]
        public async Task<IActionResult> GetUserCart(cartToOrder cartTo)
        {
                    var userid = GetUserId();
                    var cartDetail = await _context.cartDetails.FindAsync(cartTo.Id);
                    if (cartDetail != null)
                    {
                        using var transaction = _context.Database.BeginTransaction();
                        try
                        {
                            var applicationUser = _context.applicationUsers.Find(userid);
                          
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
                                unitPrice = product.price.Value
                            };
                            _context.orderDetails.Add(CTDH);
                             _context.cartDetails.Remove(cartDetail);
                            _context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                        }

                    }
            return Content("Đặt hàng thành công");
        }
        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem=await _cartService.GetCartItemCount();
            return Ok(cartItem);
        }

        //public async Task<IActionResult> checkout()
        //{
        //    bool isCheckOut = await _cartService.Docheckout();
        //    if (isCheckOut==false)
        //    {
        //        throw new Exception("Something happen in server side");
        //    }
        //    return RedirectToAction("Index", "Home");

        //}

    }
}
