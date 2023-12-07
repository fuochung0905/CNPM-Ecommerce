using CNPM_ktxUtc2Store.Controllers.Constants;
using CNPM_ktxUtc2Store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<applicationUser> _usermanagement;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(ApplicationDbContext context, UserManager<applicationUser> usermanagement, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _usermanagement = usermanagement;
            _roleManager = roleManager;

        }
        public async Task<IActionResult> wantToSaler(string? id)
        {
           var user = await _context.applicationUsers.FindAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> wantToSaler(string id,applicationUser user)
        {
            var userId = GetUserId();
          
            if (user.isUprole == false)
            {
                var userdb = await _context.applicationUsers.FindAsync(userId);
                userdb.isUprole = true;
                _context.applicationUsers.Update(userdb);
                _context.SaveChanges();
                return Content("Đã gửi thành công chờ duyệt");
            }
            return Content("Bạn đã là người bán hàng");

        }


        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var user = await _context.applicationUsers.FindAsync(userId);
            var userInDb = await _usermanagement.FindByEmailAsync(user.Email);
            if (userInDb != null)
            {
                await _usermanagement.AddToRoleAsync(user, Roles.Saler.ToString());
                await _usermanagement.RemoveFromRoleAsync(user, Roles.User.ToString());
                return Content("success");

            }


            return Content("fail");
        }
    }
}
