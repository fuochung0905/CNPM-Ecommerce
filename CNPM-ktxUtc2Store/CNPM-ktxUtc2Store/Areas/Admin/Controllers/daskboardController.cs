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
            var nvroles = _context.Roles.Where(x => x.Name == "Saler").ToList();
            dask dask = new dask();
            foreach(var role in nvroles)
            {
                int countnv = _context.UserRoles.Where(x => x.RoleId == role.Id).ToList().Count();
                dask.sonhanvien = countnv;
            }
            var userroles= _context.Roles.Where(x => x.Name == "User").ToList();
            foreach(var role in userroles)
            {
                int countuser = _context.UserRoles.Where(x => x.RoleId == role.Id).ToList().Count();
                dask.songuoidung = countuser;
            }
           var order=_context.orders.Include(x=>x.applicationUser).Include(x=>x.orderDetails).ThenInclude(o=>o.product).Where(x=>x.IsComplete).ToList();
            double tongdoanhthu = 0.0;
            foreach(var item in order)
            {
                var orderdetail = _context.orderDetails.Find(item.Id);
                tongdoanhthu = tongdoanhthu + (orderdetail.unitPrice * orderdetail.quantity);
                dask.order.Add(item);
                foreach(var i in item.orderDetails) {
                    dask.orderDetail.Add(i);
                }
            }
            dask.tongdoanhso = tongdoanhthu;
            var products = _context.products.ToList();
            double gianhap = 0.0;
            foreach(var pr in products) {
                gianhap = pr.oldprice * pr.soluongnhap + gianhap;
            }
            dask.tongnhaphang=gianhap;

          
            return View(dask);
        }
    }
}
