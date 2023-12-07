using CNPM_ktxUtc2Store.Areas.Admin.dto;
using CNPM_ktxUtc2Store.Controllers.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<applicationUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<applicationUser> usermanagement)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _usermanagement = usermanagement;
            _httpContextAccessor= httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.applicationUsers.Where(x => x.isUprole == true).ToListAsync();
            changeRole a = new changeRole();
            foreach(var user in applicationDbContext) { 
            a.applicationUsers.Add(user);
            }
            return View(a);
        }
        [HttpPost]
        public async Task<IActionResult> Index(changeRole user)
        {
            var userdb = await _context.applicationUsers.FindAsync(user.Id);
            var userInDb = await _usermanagement.FindByEmailAsync(userdb.Email);
            userdb.isSale = true;
            _context.applicationUsers.Update(userdb);
            _context.SaveChanges();
            if (userInDb != null)
            {
                await _usermanagement.AddToRoleAsync(userdb, Roles.Saler.ToString());
                await _usermanagement.RemoveFromRoleAsync(userdb, Roles.User.ToString());
                return Content("success");

            }
          
            return RedirectToAction("Index", "Home");
        }
    }
}
