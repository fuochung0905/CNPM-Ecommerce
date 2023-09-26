using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Service.Impl
{
    public class UserOrderService:IUserOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderService(ApplicationDbContext context, UserManager<IdentityUser> usermanagement, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _usermanagement = usermanagement;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<order>> UserOrders()
        {
            var userid = GetUserId();
            if(string.IsNullOrWhiteSpace(userid)) 
            {
                throw new Exception("User is not logged-in");
            }

            var orders =await _context.orders
                .Include(x=>x.status)
                .Include(x=>x.orderDetails)
                .ThenInclude(x=>x.product)
                .ThenInclude(x=>x.category)
                .Where(a=>a.userId==userid).ToListAsync();
             return orders;
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }
    }
}
