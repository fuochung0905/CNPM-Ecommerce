using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Data.Components
{
    public class footer1ViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public footer1ViewComponent(ApplicationDbContext dbContext)
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
