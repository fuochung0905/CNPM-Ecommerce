using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Data.Components
{
    public class AdressViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<applicationUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdressViewComponent(ApplicationDbContext dbContext, UserManager<applicationUser> usermanagement, IHttpContextAccessor httpContextAccessor)
        {
            _context = dbContext;
            _usermanagement = usermanagement;
            _httpContextAccessor = httpContextAccessor;
        }

        public  IViewComponentResult Invoke()
        {
            var userId = GetUserId();
           var useradress= _context.userAdresses.Include(x=>x.applicationUser).Include(x=>x.adress).Where(x=>x.applicationUserId.Equals(userId)).Where(x=>x.isDefine==true).ToList();    
            return View(useradress);
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }
    }
}
