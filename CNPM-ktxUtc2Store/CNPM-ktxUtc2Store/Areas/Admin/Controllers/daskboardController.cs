using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNPM_ktxUtc2Store.Areas.Admin.dto;
using Microsoft.AspNetCore.Authorization;

namespace CNPM_ktxUtc2Store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class daskboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public daskboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            dask dask = new dask();
            int countnv = _context.UserRoles.Where(x => x.RoleId == "56591a2f-17a4-4dd9-8642-8b4c45471a99").ToList().Count();
            dask.sonhanvien = countnv;
            int countuser = _context.UserRoles.Where(x => x.RoleId == "1aedbdb7-0423-4765-990f-9694a86aa4d9").ToList().Count();
            dask.songuoidung = countuser;
           var order=_context.orders.Where(x=>x.IsComplete).ToList();
            double tongdoanhthu = 0.0;
            foreach(var item in order)
            {
                var orderdetail = _context.orderDetails.Find(item.Id);
                tongdoanhthu = tongdoanhthu + (orderdetail.unitPrice * orderdetail.quantity);
            }
            dask.tongdoanhso = tongdoanhthu;
            return View(dask);
        }
    }
}
