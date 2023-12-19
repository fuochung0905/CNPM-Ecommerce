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
           var order=_context.orders.OrderByDescending(x=>x.Id).Include(x=>x.applicationUser).Include(x=>x.orderDetails).ThenInclude(o=>o.product).Where(x=>x.IsComplete == true).ToList();
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
           
            var month = _context.orders.Where(x => x.updateDate.Month == DateTime.Now.Month).Where(x => x.IsComplete==true).Include(x => x.applicationUser).Include(x => x.orderDetails).ThenInclude(o => o.product).ToList();
           
            double doanhthuthang = 0.0;
            foreach (var item in month)
            {
              
                var orderdetail = _context.orderDetails.Find(item.Id);
                doanhthuthang = doanhthuthang + (orderdetail.unitPrice * orderdetail.quantity);
               
            }
            dask.doanhthuthang = doanhthuthang;
            var date = _context.orders.Where(x => x.updateDate.Day == DateTime.Now.Day).Where(x => x.IsComplete == true).Include(x => x.applicationUser).Include(x => x.orderDetails).ThenInclude(o => o.product).ToList();
            double doanhthungay = 0.0;
            foreach (var item in date)
            {
                var orderdetail = _context.orderDetails.Find(item.Id);
                doanhthungay = doanhthungay + (orderdetail.unitPrice * orderdetail.quantity);

            }
            dask.doanhthungay = doanhthungay;


            var year = _context.orders.Where(x => x.updateDate.Year == DateTime.Now.Year).Where(x => x.IsComplete == true).Include(x => x.applicationUser).Include(x => x.orderDetails).ThenInclude(o => o.product).ToList();
            double doanhthunam = 0.0;
            foreach (var item in year)
            {
                var orderdetail = _context.orderDetails.Find(item.Id);
                doanhthunam = doanhthunam + (orderdetail.unitPrice * orderdetail.quantity);

            }
            dask.doanhthunam = doanhthunam;


            var donchoduyet = _context.orders.Include(x => x.orderDetails).Where(x => x.IsDelete ==false ).ToList();
            int dem = donchoduyet.Count();
            dask.donchoduyet = dem;

            var donthanhcong = _context.orders.Include(x => x.orderDetails).Where(x => x.IsComplete == true).ToList();
            int dem1 = donthanhcong.Count();
            dask.thanhcong = dem1;
            return View(dask);
        }
    }
}
