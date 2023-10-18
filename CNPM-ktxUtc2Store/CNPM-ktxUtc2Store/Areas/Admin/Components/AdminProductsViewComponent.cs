using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Areas.Admin.Components
{
    public class AdminProductsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public AdminProductsViewComponent(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public IViewComponentResult Invoke()
        {
            var product = _context.products.ToList();
            return View(product);
        }
    }
}
