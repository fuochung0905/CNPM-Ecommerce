  using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using X.PagedList;

namespace CNPM_ktxUtc2Store.Service.Impl
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _dbcontext;
        public HomeService(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;

        }
        public List<category> Categories()
        {
            return  _dbcontext.categories.ToList();
        }
        public List<product> GetProduct(/*string sterm = "", int categoryId = 0)*/)
        {
            //sterm = sterm.ToLower();

            List<product> products =  (from product in _dbcontext.products
                                                   join category in _dbcontext.categories
                                                   on product.categoryId equals category.Id
                                                  //where string.IsNullOrWhiteSpace(sterm) ||( product != null && product.productName.ToLower().Contains(sterm))
                                                   select new product
                                                   {
                                                       Id = product.Id,
                                                       productName = product.productName,
                                                       description = product.description,
                                                       oldprice = product.oldprice,
                                                       price = product.price,
                                                       categoryId=product.categoryId,
                                                       imageUrl = product.imageUrl,
                                                     

                                                   }).ToList();
            //if (categoryId > 0)
            //{
            //    products = products.Where(a => a.categoryId == categoryId).ToList();
            //}
         
            return products;

        }
    }
}
