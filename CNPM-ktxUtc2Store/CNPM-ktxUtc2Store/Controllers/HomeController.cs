using CNPM_ktxUtc2Store.Dto;
using CNPM_ktxUtc2Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using X.PagedList;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CNPM_ktxUtc2Store.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeRepository;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<IdentityUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHomeService homeRepository, ApplicationDbContext dbcontext, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _homeRepository = homeRepository;
            _dbcontext = dbcontext;
            _usermanagement = userManager;
            _httpContextAccessor = httpContextAccessor; 
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
                               where(product.qty_inStock > 0)
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
                                   where (product.qty_inStock > 0)
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
                                   where (product.qty_inStock > 0)
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
                                   where (product.qty_inStock > 0)
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
      

        public IActionResult Details(int? id)
        {
            if (id == null || _dbcontext.products == null)
            {
                return NotFound();
            }

            var product = new product();
            product = _dbcontext.products.Find(id);
            
            var productvariation = (from p in _dbcontext.products
                        join pv in _dbcontext.productVariations
                        on p.Id equals pv.productId
                        where (p.Id == id)
                        select new productVariation
                        {
                            product = p,
                            variation = pv.variation,
                        }).ToList();
            product.ProductVariations = productvariation;

            var result = new productvariatonOrderView();

            result.product=product;
            return View(result);
        }
        [HttpPost]
        public IActionResult Details(int id,productvariatonOrderView model)
        {
            if (string.IsNullOrEmpty(model.color))
            {
                model.color = "";
            }
            if (string.IsNullOrEmpty(model.size))
            {
                model.size = "";
            }
            using var transaction = _dbcontext.Database.BeginTransaction();
            var userid = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userid))
                {
                    return Redirect("/Identity/Account/Login");
                }
                //var dathang =  GetDatHang(userid);
               
                  var dathang = new order
                    {
                        userId = userid,
                        createDate = DateTime.UtcNow,
                        orderStatusId = 1
                    };
                    _dbcontext.orders.Add(dathang);
               
                _dbcontext.SaveChanges();
                var CTDH = _dbcontext.orderDetails.FirstOrDefault(x => x.orderId == dathang.Id );
                    var product = _dbcontext.products.Find(id);
                    CTDH = new orderDetail
                    {
                        productId = id,
                        orderId = dathang.Id,
                        quantity = model.quantity,
                        size=model.size,
                        color=model.color,
                        unitPrice = product.price
                    };
                    _dbcontext.orderDetails.Add(CTDH);
                product.qty_inStock = product.qty_inStock - model.quantity;
              
                _dbcontext.Update(product);
                _dbcontext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
            }

            return RedirectToAction("Userorder", "UserOrder");
            
        }
        public  order GetDatHang(string userId)
        {
            var dathang = _dbcontext.orders.FirstOrDefault(x => x.userId == userId);
            return dathang;
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
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