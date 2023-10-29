using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Areas.Admin.Components
{
    public class adminViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<applicationUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public adminViewComponent(ApplicationDbContext dbContext, UserManager<applicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = dbContext;
            _usermanagement = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public IViewComponentResult Invoke()
        {

            string userId = GetUserId();
            var user = _context.applicationUsers.Find(userId);
            return View(user);
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }
    }
}
