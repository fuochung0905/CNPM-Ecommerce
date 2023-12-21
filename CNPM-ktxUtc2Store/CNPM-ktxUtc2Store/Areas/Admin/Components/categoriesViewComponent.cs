using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Areas.Admin.Components
{
    public class categoriesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public categoriesViewComponent(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public IViewComponentResult Invoke()
        {
            var product = _context.categories.ToList();
            return View(product);
        }
    }
}
