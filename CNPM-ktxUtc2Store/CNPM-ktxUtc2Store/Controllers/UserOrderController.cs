using CNPM_ktxUtc2Store.Service;
using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly IUserOrderService _userOrderService;
        public UserOrderController(IUserOrderService userOrderService)
        {
            _userOrderService = userOrderService;
            
        }
        public async Task<IActionResult> Userorder()
        {
            var orders = await _userOrderService.UserOrders();
            return View(orders);
        }
        public IActionResult Order(int productId) {
            return View("UserOrder", "Userorder");
        }
    }
}
