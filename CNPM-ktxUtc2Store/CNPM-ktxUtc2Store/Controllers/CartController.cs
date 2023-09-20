using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public async Task<IActionResult> AddItem(int productId, int quantity=1,int redirect=0)
        {
            var cartCount = await _cartService.AddItem(productId, quantity);
            if(redirect == 0) { 
                return Ok(cartCount);
            }
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> Remove(int productId)
        {
            var cartCount= await _cartService.RemoveItem(productId);

            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart= await _cartService.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem=await _cartService.GetCartItemCount();
            return Ok(cartItem);
        }

    }
}
