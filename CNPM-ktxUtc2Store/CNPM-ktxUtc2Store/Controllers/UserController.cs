using CNPM_ktxUtc2Store.Controllers.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace CNPM_ktxUtc2Store.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
    
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<applicationUser> _userManager;
        private readonly IUserStore<applicationUser> _userStore;
        private readonly IUserEmailStore<applicationUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<applicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(ApplicationDbContext context, 
            UserManager<applicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IUserStore<applicationUser> userStore,
            IUserEmailStore<applicationUser> emailStore,
            IEmailSender emailSender ,
            SignInManager<applicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _userStore = userStore;
            _emailStore = emailStore;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _roleManager = roleManager;


        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(pricipal);

            return userId;
        }




        public async Task<IActionResult> Index()
        {
            
            await _roleManager.CreateAsync(new IdentityRole(Roles.Saler.ToString()));
            var userID = GetUserId();
            var user = await _context.applicationUsers.FindAsync(userID);
            var saler = new applicationUser
            {
                fullname = user.fullname,
                profilePicture = user.profilePicture,
                UserName = "",
                Email =user.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true

            };

            var userInDb = await _userManager.FindByEmailAsync(saler.Email);
            if (userInDb == null)
            {
                await _userManager.CreateAsync(saler,"12345");
                await _userManager.AddToRoleAsync(saler, Roles.Saler.ToString());

            }
            return Content("Thanh cong");
        }
       
    }
}
