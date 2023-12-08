using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Data.Components
{
    public class adressstorageViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public adressstorageViewComponent(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public IViewComponentResult Invoke()
        {
            var categories = _context.AdressStorage.ToList();
            return View(categories);
        }
    }
}

