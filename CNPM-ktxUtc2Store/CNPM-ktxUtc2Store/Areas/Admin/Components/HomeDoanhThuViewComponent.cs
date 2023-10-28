using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Areas.Admin.Components
{
    public class HomeDoanhThuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public HomeDoanhThuViewComponent(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public IViewComponentResult Invoke()
        {
            
            var order = _context.orders.Where(x=>x.IsDelete==true).Include(x=>x.orderDetails).ToList();
            
            return View(order);
        }
    }
}
