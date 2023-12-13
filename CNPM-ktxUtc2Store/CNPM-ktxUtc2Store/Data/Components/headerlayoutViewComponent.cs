using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Data.Components
{
    public class headerlayoutViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public headerlayoutViewComponent(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public IViewComponentResult Invoke()
        {
            var categories = _context.InforStorage.ToList();
            return View(categories);
        }
    }
}
