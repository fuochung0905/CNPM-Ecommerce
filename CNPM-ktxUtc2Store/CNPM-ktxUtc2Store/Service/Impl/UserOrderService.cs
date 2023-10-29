using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Service.Impl
{
    public class UserOrderService:IUserOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<applicationUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderService(ApplicationDbContext context, UserManager<applicationUser> usermanagement, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _usermanagement = usermanagement;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<order>> UserOrders()
        {
            var userid = GetUserId();
            if(string.IsNullOrWhiteSpace(userid)) 
            {
                throw new Exception("User is not logged-in");
            }

            var orders =await _context.orders
                .Include(x=>x.status)
                .Include(x=>x.orderDetails)
                .ThenInclude(x=>x.product)
                .ThenInclude(x=>x.category)
                .Where(a=>a.applicationUser.Id ==userid).ToListAsync();
             return orders; 
        }
        public async Task<int>XoaDatHang(int productId)
        {
            var userid = GetUserId();
            try 
            {
                if (string.IsNullOrEmpty(userid))
                {
                    throw new Exception("user is not logged-in");
                }

                var order = await GetDatHang(userid);
                if (order is null)
                {
                    throw new Exception("order is empty");
                }
                var CTDH = _context.orderDetails.FirstOrDefault(a => a.orderId == order.Id && a.productId == productId);
                if (CTDH is null)
                {
                    throw new Exception("Not item in order");

                }
                else if (CTDH.quantity == 1)
                {
                    _context.orderDetails.Remove(CTDH);

                }
                else
                {
                    CTDH.quantity = CTDH.quantity - 1;
                }
                _context.SaveChanges();
            }
            catch (Exception )
            {

            }
            var CTDHCOUNT = await GetCTDHCount(userid);
            return CTDHCOUNT;

        }
        //public async Task<int> Dathang(int productid, int quantity)
        //{
        //    using var transaction = _context.Database.BeginTransaction();
        //    var userid = GetUserId();
        //    try
        //    {
        //        if (string.IsNullOrEmpty(userid))
        //        {
        //            throw new Exception("user is not logged-in");
        //        }
        //        var dathang = await GetDatHang(userid);
               
        //        if(dathang is null)
        //        {
        //            dathang = new order
        //            {
        //                userId = userid,
        //                createDate = DateTime.UtcNow,
        //                orderStatusId = 1
        //            };
        //            _context.orders.Add(dathang);
        //        }
        //        _context.SaveChanges();
        //        var CTDH = _context.orderDetails.FirstOrDefault(x => x.orderId == dathang.Id && x.productId == productid);
        //        if(CTDH is not null){
        //            CTDH.quantity = CTDH.quantity+quantity;
        //        }
        //        else
        //        {
        //            var product = _context.products.Find(productid);
        //            CTDH = new orderDetail
        //            {
        //                productId = productid,
        //                orderId = dathang.Id,
        //                quantity = quantity,
        //                unitPrice = product.price
        //            };
        //            _context.orderDetails.Add(CTDH);
        //        }
        //        _context.SaveChanges();
        //        transaction.Commit();
        //    }
        //    catch (Exception )
        //    {
             
        //    }
        //    var CTDHCOUNT = await GetCTDHCount(userid);
        //    return CTDHCOUNT;

        //}
        public async Task<order>GetDatHang( string userId)
        {
            var dathang=  _context.orders.FirstOrDefault(x=>x.applicationUser.Id ==userId);
            return dathang;
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }
        public async Task<int>GetCTDHCount(string userId="")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }

            var data = await (from order in _context.orders
                              join orderDetail in _context.orderDetails
                              on order.Id equals orderDetail.orderId
                              select new { orderDetail.Id }
                              ).ToListAsync();
            return data.Count;
        }
    }
}
