using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Data.Components
{
    public class bannerViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public bannerViewComponent(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public IViewComponentResult Invoke()
        {
            var banner = _context.BannerStorage.ToList();
            return View(banner);
        }
    }
}
