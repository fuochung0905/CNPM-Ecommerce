using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
