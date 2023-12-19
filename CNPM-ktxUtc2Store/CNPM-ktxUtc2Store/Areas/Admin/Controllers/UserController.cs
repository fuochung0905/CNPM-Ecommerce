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
        public async Task<IActionResult> Saler()
        {
            UserDto userDto = new UserDto();
           var Salerrolers=_context.Roles.Where(x=>x.Name=="Saler").ToList();
            foreach(var item in Salerrolers)
            {
                foreach(var userrole in Salerrolers)
                {
                    var salers=await _context.UserRoles.Where(x=>x.RoleId==item.Id).ToListAsync();
                  foreach(var user in salers)
                    {
                        var saler= await _context.Users.Where(x=>x.Id==user.UserId).ToListAsync();
                        foreach(var i in saler)
                        {
                            userDto.Users.Add(i);
                        }
                    }
                }
            }
            return View(userDto);
        }
        public async Task<IActionResult> user()
        {
            UserDto userDto = new UserDto();
            var Salerrolers = _context.Roles.Where(x => x.Name == "User").ToList();
            foreach (var item in Salerrolers)
            {
                foreach (var userrole in Salerrolers)
                {
                    var salers = await _context.UserRoles.Where(x => x.RoleId == item.Id).ToListAsync();
                    foreach (var user in salers)
                    {
                        var saler = await _context.Users.Where(x => x.Id == user.UserId).ToListAsync();
                        foreach (var i in saler)
                        {
                            userDto.Users.Add(i);
                        }
                    }
                }
            }
            return View(userDto);
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
            var nv = new applicationUser
            {
                fullname = applicationUser.fullname,
                profilePicture = uniqueFileName,
                UserName =applicationUser.UserName,
                Email =applicationUser.Email,
                PhoneNumber=applicationUser.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true


            };

            var userInDb = await _usermanagement.FindByEmailAsync(nv.Email);
          
            if (userInDb == null)
            {
                await _usermanagement.CreateAsync(nv, "nhanvien@123");
                await _usermanagement.AddToRoleAsync(nv, Roles.Saler.ToString());
                 _context.applicationUsers.Add(nv);
                _context.SaveChanges();
                return RedirectToAction("Index", "daskboard");

            }
           return Content("Thêm thất bại");
           
        }
    }
}
