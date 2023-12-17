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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,
            UserManager<applicationUser> usermanagement,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _usermanagement = usermanagement;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
    
        private string uploadImage(applicationUser model)
        {
            string uniqueFileName = string.Empty;
            if (model.Picture != null)
            {
                string uploadFoder = Path.Combine(_webHostEnvironment.WebRootPath, "images/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;
                string filePath = Path.Combine(uploadFoder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(applicationUser applicationUser)
        {
            string uniqueFileName = uploadImage(applicationUser);

            applicationUser.profilePicture = uniqueFileName;
            applicationUser.PhoneNumberConfirmed = true;
            applicationUser.EmailConfirmed = true;
            var userInDb = await _usermanagement.FindByEmailAsync(applicationUser.Email);
            if (userInDb == null)
            {
                await _usermanagement.CreateAsync(applicationUser, "nhanvien@123");
                await _usermanagement.AddToRoleAsync(applicationUser, Roles.Saler.ToString());
                _context.applicationUsers.Add(applicationUser);
                _context.SaveChanges(); 
                return RedirectToAction("Index", "daskboard");

            }
           return Content("Thêm thất bại");
           
        }
    }
}
