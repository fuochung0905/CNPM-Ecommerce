using CNPM_ktxUtc2Store.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;

namespace CNPM_ktxUtc2Store.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly IUserOrderService _userOrderService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<applicationUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderController(IUserOrderService userOrderService, ApplicationDbContext context, UserManager<applicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userOrderService = userOrderService;
            _httpContextAccessor = httpContextAccessor;
            _usermanagement = userManager;

        }
        public async Task<IActionResult> GetTotalmyOder()
        {
            var userId = GetUserId();
           var order=await _context.orders.Where(x=>x.applicationUserId == userId).ToListAsync();
            int dem = order.Count();
            return Ok(dem);
        }

        public async Task<IActionResult> GetTotalmyOrderWait()
        {
            var userId = GetUserId();
            var order = await _context.orders.Where(x => x.applicationUserId == userId).Where(x=>x.IsDelete==true).Where(x=>x.IsComplete==false).Where(x=>x.isHuy==false).ToListAsync();
            int dem = order.Count();
            return Ok(dem);
        }
        public async Task<IActionResult> GetTotalmyOrderComplete()
        {
            var userId = GetUserId();
            var order = await _context.orders.Where(x => x.applicationUserId == userId).Where(x => x.IsDelete == true).Where(x=>x.IsComplete==true).Where(x=>x.isHuy==false).ToListAsync();
            int dem = order.Count();
            return Ok(dem);
        }
        public async Task<IActionResult> GetTotalmyOrderCancle()
        {
            var userId = GetUserId();
            var order = await _context.orders.Where(x => x.applicationUserId == userId).Where(x => x.isHuy == true).ToListAsync();
            int dem = order.Count();
            return Ok(dem);
        }
        public async Task<IActionResult> myOrder()
        {
            var userId = GetUserId();
            var order = await _context.orders.Where(x => x.applicationUserId == userId).ToListAsync();
            myOrderDto myOrderDto = new myOrderDto();
            foreach (var orderItem in order)
            {
                var orderdetail = await _context.orderDetails.Include(x => x.order).ThenInclude(o => o.status).Include(x => x.product).Where(x => x.orderId == orderItem.Id).ToListAsync();
                foreach (var item in orderdetail)
                {
                    myOrderDto.orderDetails.Add(item);
                }
            }
            return View(myOrderDto);
        }

        public async Task<shoppingCart> GetUserCart()
        {
            var userId = GetUserId();

            if (userId == null)
                throw new Exception("Invalid user");
            var shoppingcart = await _context.shoppingCarts
                .Include(a => a.cartDetails)
                .ThenInclude(a => a.product)
                .ThenInclude(a => a.category)
                .Where(a => a.applicationUserId == userId).FirstOrDefaultAsync();
            return shoppingcart;
        }
        public IActionResult huydon(int id)
        {
        var order = _context.orders.Where(x=>x.Id== id).FirstOrDefault();
            if(order != null)
            {
                order.isHuy=true;
                var status = _context.orderStatus.Where(x => x.statusName == "Đã hủy").ToList();
                foreach (var item in status)
                {
                    order.status = item;
                }
                _context.orders.Update(order);
                _context.SaveChanges();
                return RedirectToAction("myOrder", "UserOrder");

            }
            return Content("Null");
;          
        }
        public async Task<IActionResult> Userorder()
         {
            var cart = await GetUserCart();
            cartToOrder a = new cartToOrder();
            a.shoppingCarts = cart;

            return View(a);
        }
    
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Userorder(cartToOrder cartTo)
        {
            var userid = GetUserId();

            var userAdress= await _context.userAdresses.Include(x=>x.adress).Include(x=>x.applicationUser)
                .Where(x=>x.applicationUserId==userid).Where(x=>x.isDefine==true).ToListAsync();
            foreach( var item in userAdress)
            {
                if(item != null)
                {
                    var cartDetail = await _context.cartDetails.FindAsync(cartTo.Id);
                    if (cartDetail != null)
                    {
                        using var transaction = _context.Database.BeginTransaction();
                        try
                        {
                            var useradress = await _context.userAdresses.Include(x => x.adress).Where(x => x.isDefine == true).Where(x => x.applicationUserId == GetUserId()).ToListAsync();
                            var applicationUser = _context.applicationUsers.Find(userid);
                            if (applicationUser.PhoneNumber == null)
                            {
                                return Redirect("/Identity/Account/Manage");
                            }
                            string address = "";
                            foreach(var au in useradress)
                            {
                                address = au.adress.homeAdress + ", " + au.adress.villageAdress + ", " + au.adress.districAdress ;
                            }
                            var dathang = new order
                            {
                                applicationUserId = userid,
                                createDate = DateTime.Now,
                                updateDate = DateTime.Now,
                                orderStatusId = 1,
                                IsComplete = false,
                                IsDelete=false
                            };
                            _context.orders.Add(dathang);
                            _context.SaveChanges();
                            var CTDH = _context.orderDetails.FirstOrDefault(x => x.orderId == dathang.Id);
                            var product = _context.products.Find(cartDetail.productId);
                            CTDH = new orderDetail
                            {
                                Id=dathang.Id,
                                productId = cartDetail.productId,
                                orderId = dathang.Id,
                                quantity = cartDetail.quantity,
                                size = cartDetail.size,
                                color = cartDetail.color,
                                addressuer=address,
                                unitPrice = product.price.Value
                            };
                            _context.orderDetails.Add(CTDH);
                            _context.cartDetails.Remove(cartDetail);
                            product.qty_inStock = product.qty_inStock - cartDetail.quantity;
                            product.daban = cartDetail.quantity;
                            _context.products.Update(product);
                            _context.SaveChanges();
                            transaction.Commit();
                            await SendEmailAsync(applicationUser.Email, "Cảm ơn quý khách đã đặt hàng tại UTC2Store",
                      $"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"100%\" width=\"100%\">\r\n" +
                      $"    <tbody>\r\n" +
                      $"        <tr>\r\n" +
                      $"            <td align=\"center\" valign=\"top\">\r\n" +
                      $"                <div>\r\n" +
                      $"                </div>\r\n" +
                      $"                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" style=\"background-color:#ffffff;border:1px solid #dedede;border-radius:3px\">\r\n" +
                      $"                    <tbody>\r\n" +
                      $"                        <tr>\r\n" +
                      $"                            <td align=\"center\" valign=\"top\">\r\n\r\n" +
                      $"                                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"background-color:#e74c3c;color:#ffffff;border-bottom:0;font-weight:bold;line-height:100%;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;border-radius:3px 3px 0 0\">\r\n" +
                      $"                                    <tbody>\r\n                                        <tr>\r\n                                            <td style=\"padding:36px 48px;display:block\">\r\n" +
                      $"                                                <h1 style=\"font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;font-size:30px;font-weight:300;line-height:150%;margin:0;text-align:left;color:#ffffff;background-color:inherit\">Đơn hàng mới: #{dathang.Id}</h1>\r\n" +
                      $"                                            </td>\r\n" +
                      $"                                        </tr>\r\n" +
                      $"                                    </tbody>\r\n" +
                      $"                                </table>\r\n\r\n" +
                      $"                            </td>\r\n" +
                      $"                        </tr>\r\n" +
                      $"                        <tr>\r\n" +
                      $"                            <td align=\"center\" valign=\"top\">\r\n\r\n" +
                      $"                                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n" +
                      $"                                    <tbody>\r\n" +
                      $"                                        <tr>\r\n" +
                      $"                                            <td valign=\"top\" style=\"background-color:#ffffff\">\r\n\r\n" +
                      $"                                                <table border=\"0\" cellpadding=\"20\" cellspacing=\"0\" width=\"100%\">\r\n" +
                      $"                                                    <tbody>\r\n" +
                      $"                                                        <tr>\r\n" +
                      $"                                                            <td valign=\"top\" style=\"padding:48px 48px 32px\">\r\n" +
                      $"                                                                <div style=\"color:#636363;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;font-size:14px;line-height:150%;text-align:left\">\r\n\r\n" +
                      $"                                                                    <p style=\"margin:0 0 16px\">Bạn vừa nhận được đơn hàng từ  UTC2Store. Đơn hàng như sau:</p>\r\n" +
                      $"                                                                    <h2 style=\"color:#e74c3c;display:block;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;font-size:18px;font-weight:bold;line-height:130%;margin:0 0 18px;text-align:left\">\r\n" +
                      $"                                                                        <a href=\"#\" style=\"font-weight:normal;text-decoration:underline;color:#e74c3c\" target=\"_blank\">[Đơn hàng #{dathang.Id}]</a> ({dathang.createDate})\r\n" +
                      $"                                                                    </h2>\r\n\r\n" +
                      $"                                                                    <div style=\"margin-bottom:40px\">\r\n" +
                      $"                                                                        <table cellspacing=\"0\" cellpadding=\"6\" border=\"1\" style=\"color:#636363;border:1px solid #e5e5e5;vertical-align:middle;width:100%;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\">\r\n" +
                      $"                                                                            <thead>\r\n" +
                      $"                                                                                <tr>\r\n" +
                      $"                                                                                    <th scope=\"col\" style=\"color:#636363;border:1px solid #e5e5e5;vertical-align:middle;padding:12px;text-align:left\">Sản phẩm</th>\r\n" +
                      $"                                                                                    <th scope=\"col\" style=\"color:#636363;border:1px solid #e5e5e5;vertical-align:middle;padding:12px;text-align:left\">Số lượng</th>\r\n" +
                      $"                                                                                    <th scope=\"col\" style=\"color:#636363;border:1px solid #e5e5e5;vertical-align:middle;padding:12px;text-align:left\">Giá</th>\r\n" +
                      $"                                                                                </tr>\r\n" +
                      $"                                                                            </thead>\r\n" +
                      $"                                                                            <tbody>\r\n" +
                      $"<tr> <td> {product.productName}</td> <td> {CTDH.quantity}</td> <td> {CTDH.unitPrice}</td></tr>" +
                      $"                                                                               \r\n" +
                      $"                                                                            </tbody>\r\n" +
                      $"                                                                            <tfoot>\r\n" +
                      $"                                                                                <tr>\r\n" +
                      $"                                                                                    <th scope=\"row\" colspan=\"2\"\r\n" +
                      $"                                                                                        style=\"color:#636363;border:1px solid #e5e5e5;vertical-align:middle;padding:12px;text-align:left;border-top-width:4px\">\r\n" +
                      $"                                                                                        Nguyên giá:\r\n" +
                      $"                                                                                    </th>\r\n" +
                      $"                                                                                    <td style=\"color:#636363;border:1px solid #e5e5e5;vertical-align:middle;padding:12px;text-align:left;border-top-width:4px\">\r\n" +
                      $"                                                                                        <span>{CTDH.unitPrice}&nbsp;<span>₫</span></span>\r\n" +
                      $"                                                                                    </td>\r\n" +
                      $"                                                                                </tr>\r\n" +
                      $"                                                                              \r\n" +
                      $"                                                                                <tr>\r\n" +
                      $"                                                                                    <th scope=\"row\" colspan=\"2\"\r\n" +
                      $"                                                                                        style=\"color:#636363;border:1px solid #e5e5e5;vertical-align:middle;padding:12px;text-align:left\">\r\n" +
                      $"                                                                                        Tổng cộng:\r\n" +
                      $"                                                                                    </th>\r\n" +
                      $"                                                                                    <td style=\"color:#636363;border:1px solid #e5e5e5;vertical-align:middle;padding:12px;text-align:left\">\r\n " +
                      $"                                                                                       <span>{CTDH.unitPrice*CTDH.quantity}&nbsp;<span>₫</span></span>\r\n" +
                      $"                                                                                    </td>\r\n" +
                      $"                                                                                </tr>\r\n" +
                      $"                                                                            </tfoot>\r\n" +
                      $"                                                                        </table>\r\n" +
                      $"                                                                    </div>\r\n\r\n\r\n" +
                      $"                                                                    <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"width:100%;vertical-align:top;margin-bottom:40px;padding:0\">\r\n" +
                      $"                                                                        <tbody>\r\n" +
                      $"                                                                        <td valign=\"top\" width=\"50%\"\r\n" +
                      $"                                                                            style=\"text-align:left;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;border:0;padding:0\">\r\n" +
                      $"                                                                            <h2 style=\"color:#e74c3c;display:block;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;font-size:18px;font-weight:bold;line-height:130%;margin:0 0 18px;text-align:left\">\r\n" +
                      $"                                                                                Thông tin người nhận\r\n" +
                      $"                                                                            </h2>\r\n\r\n" +
                      $"                                                                            <address style=\"padding:12px;color:#e74c3c;border:1px solid #e5e5e5\">\r\n" +
                      $"                                                                                {applicationUser.fullname}<br>{CTDH.addressuer} <br><a href=\"tel:{applicationUser.PhoneNumber}\"\r\n" +
                      $"                                                                                                                              style=\"color:#e74c3c;font-weight:normal;text-decoration:underline\"\r\n" +
                      $"                                                                                                                              target=\"_blank\">{applicationUser.PhoneNumber}</a> <br><a href=\"mailto:{applicationUser.Email}\"\r\n" +
                      $"                                                                                                                                                                   target=\"_blank\">{applicationUser.Email}</a>\r\n " +
                      $"                                                                           </address>\r\n" +
                      $"                                                                        </td>\r\n</tbody>\r\n" +
                      $"                                                                    </table>\r\n " +
                      $"                                                               </div>\r\n" +
                      $"                                                            </td>\r\n" +
                      $"                                                        </tr>\r\n" +
                      $"                                                    </tbody>\r\n  " +
                      $"                                              </table>\r\n\r\n" +
                      $"                                            </td>\r\n" +
                      $"                                        </tr>\r\n" +
                      $"                                    </tbody>\r\n" +
                      $"                                </table>\r\n\r\n" +
                      $"                            </td>\r\n" +
                      $"                        </tr>\r\n" +
                      $"                    </tbody>\r\n" +
                      $"                </table>\r\n" +
                      $"            </td>\r\n" +
                      $"        </tr>\r\n\r\n" +
                      $"    </tbody>\r\n</table>");


                        }
                        catch (Exception)
                        {
                        }

                    }
                    return RedirectToAction("myOrder","UserOrder");
                }
            }
            return RedirectToAction("Create","Adresses");

        }
        public async Task<bool> SendEmailAsync(string email, string subject, string confirmLink)
        {
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com"; //host name
                    smtp.Port = 587; //port number
                    smtp.EnableSsl = true; //whether your smtp server requires SSL
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = "h09052003n@gmail.com",
                        Password = "debskzkkbtkmqdfe"
                    };
                }
                MailAddress fromAddress = new MailAddress("h09052003n@gmail.com", "UTC2Store");
                message.From = fromAddress;
                message.To.Add(email);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = confirmLink;
                smtp.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<order> GetDatHang(string userId)
        {
            var dathang = _context.orders.FirstOrDefault(x => x.applicationUser.Id == userId);
            return dathang;
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }
        public async Task<int> GetCTDHCount(string userId = "")
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
        public double tongtien(doneOrder doneOrder)
        {
           var orderdetail= _context.orderDetails.Where(x=>x.orderId==doneOrder.orderId).FirstOrDefault();
            if (orderdetail!=null)
            {
                return orderdetail.quantity * orderdetail.unitPrice;
            }
            return 0;
        }


    }
}
