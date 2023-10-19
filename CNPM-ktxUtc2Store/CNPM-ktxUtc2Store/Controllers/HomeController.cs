using CNPM_ktxUtc2Store.Dto;
using CNPM_ktxUtc2Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace CNPM_ktxUtc2Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeRepository;
        private readonly ApplicationDbContext _dbcontext;

        public HomeController(ILogger<HomeController> logger, IHomeService homeRepository, ApplicationDbContext dbcontext)
        {
            _logger = logger;
            _homeRepository = homeRepository;
            _dbcontext = dbcontext;
        }

        public IActionResult Index(string maloai, string searchName, int? page)
        {
            var listProduct = new List<product>();
            if (!string.IsNullOrEmpty(maloai) & !string.IsNullOrEmpty(searchName))
            {
                int cateId = Convert.ToInt32(maloai);
                listProduct = (from product in _dbcontext.products
                               join category in _dbcontext.categories
                               on product.categoryId equals category.Id
                               select new product
                               {
                                   Id = product.Id,
                                   productName = product.productName,
                                   description = product.description,
                                   discount = product.discount,
                                   price = product.price,
                                   categoryId = product.categoryId,
                                   category = category,
                                   imageUrl = product.imageUrl,
                               }).Where(x => x.categoryId == cateId & x.productName.Contains(searchName)).ToList();
            }
            else
            {
                if (string.IsNullOrEmpty(searchName) & !string.IsNullOrEmpty(maloai))
                {
                    int cateId = Convert.ToInt32(maloai);
                    listProduct = (from product in _dbcontext.products
                                   join category in _dbcontext.categories
                                   on product.categoryId equals category.Id
                                   select new product
                                   {
                                       Id = product.Id,
                                       productName = product.productName,
                                       description = product.description,
                                       discount = product.discount,
                                       price = product.price,
                                       categoryId = product.categoryId,
                                       imageUrl = product.imageUrl,
                                   }).Where(x => x.categoryId == cateId).ToList();
                }
                if (string.IsNullOrEmpty(maloai) & !string.IsNullOrEmpty(searchName))
                {
                    listProduct = (from product in _dbcontext.products
                                   join category in _dbcontext.categories
                                   on product.categoryId equals category.Id
                                   select new product
                                   {
                                       Id = product.Id,
                                       productName = product.productName,
                                       description = product.description,
                                       discount = product.discount,
                                       price = product.price,
                                       categoryId = product.categoryId,
                                       imageUrl = product.imageUrl,
                                   }).Where(x => x.productName.Contains(searchName)).ToList();
                }
                if (string.IsNullOrEmpty(maloai) & string.IsNullOrEmpty(searchName))
                {
                    listProduct = (from product in _dbcontext.products
                                   join category in _dbcontext.categories
                                   on product.categoryId equals category.Id
                                   select new product
                                   {
                                       Id = product.Id,
                                       productName = product.productName,
                                       description = product.description,
                                       discount = product.discount,
                                       price = product.price,
                                       categoryId = product.categoryId,
                                       imageUrl = product.imageUrl,
                                   }).ToList();
                }

            }


            ViewBag.maloai = maloai;
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<product> list = new PagedList<product>(listProduct, pageNumber, pageSize);
            return View(list);
        }
        public IActionResult TheoloaiSanPham(int maloai)
        {
            var listCategory = new List<category>();
            listCategory = _dbcontext.categories.Where(x => x.Id == maloai).ToList();
            return View(listCategory);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbcontext.products == null)
            {
                return NotFound();
            }

            var product = await _dbcontext.products
                .Include(p => p.category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}