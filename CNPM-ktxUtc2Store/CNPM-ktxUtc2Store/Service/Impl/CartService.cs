using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CNPM_ktxUtc2Store.Service.Impl
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(ApplicationDbContext context, UserManager<IdentityUser> usermanagement, IHttpContextAccessor httpContextAccessor)
        {

            _context = context;
            _usermanagement = usermanagement;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<int> AddItem(int productId, int quantity)
        {
            // cart -save
            //cartDetail -error
            using var transaction = _context.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user is not logged-in");
                }

                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new shoppingCart
                    {
                        UserId = userId
                    };
                    _context.shoppingCarts.Add(cart);
                }
                _context.SaveChanges();
                // cartDetail session
                var cartItem = _context.cartDetails.FirstOrDefault(a => a.shoppingCartId == cart.Id && a.productId == productId);
                if (cartItem is not null)
                {
                    cartItem.quantity += quantity;
                }
                else
                {
                    cartItem = new cartDetail
                    {
                        productId = productId,
                        shoppingCartId = cart.Id,
                        quantity = quantity
                    };
                    _context.cartDetails.Add(cartItem);
                }
                _context.SaveChanges();
                transaction.Commit();

            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;


        }
        public async Task<int> RemoveItem(int productId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user is not logged-in");
                }

                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("cart is empty");
                }
                // cartDetail session
                var cartItem = _context.cartDetails.FirstOrDefault(a => a.shoppingCartId == cart.Id && a.productId == productId);
                if (cartItem is null)
                {
                    throw new Exception("Not item in cart");

                }
                else if (cartItem.quantity == 1)
                {
                    _context.cartDetails.Remove(cartItem);

                }
                else
                {
                    cartItem.quantity = cartItem.quantity - 1;
                }
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;


        }
        public async Task<shoppingCart> GetUserCart()
        {
            var userId = GetUserId();

            if (userId == null)
                throw new Exception("Invalid user");
            var shoppingcart =await _context.shoppingCarts
                .Include(a => a.cartDetails)
                .ThenInclude(a => a.product)
                .ThenInclude(a => a.category)
                .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingcart;


        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }

            var data = await (from cart in _context.shoppingCarts
                              join cartDetail in _context.cartDetails
                              on cart.Id equals cartDetail.shoppingCartId
                              select new { cartDetail.Id }
                              ).ToListAsync();
            return data.Count;

        }



        public async Task<shoppingCart> GetCart(string userId)
        {
            var cart = _context.shoppingCarts.FirstOrDefault(x => x.UserId == userId);
            return cart;
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }
    }
}
