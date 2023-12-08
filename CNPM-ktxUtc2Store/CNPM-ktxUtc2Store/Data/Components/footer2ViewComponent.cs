using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Data.Components
{
    public class footer2ViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public footer2ViewComponent(ApplicationDbContext dbContext)
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
