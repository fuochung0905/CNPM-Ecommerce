using CNPM_ktxUtc2Store.Areas.Admin.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CNPM_ktxUtc2Store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller

    {
        private readonly ApplicationDbContext _context;
          
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            dask dask = new dask();
          int countnv= _context.Roles.Where(x=>x.Name=="Saler").Count();
            dask.sonhanvien=countnv;
            int countuser = _context.Roles.Where(x => x.Name == "User").Count();
            dask.songuoidung=countuser;
            return View(dask);
        }

    }
}
