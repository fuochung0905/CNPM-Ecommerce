using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNPM_ktxUtc2Store.Data;
using CNPM_ktxUtc2Store.Models;
using CNPM_ktxUtc2Store.Areas.Admin.dto;
using Microsoft.AspNetCore.Identity;

namespace CNPM_ktxUtc2Store.Areas.Saler.Controllers
{
    [Area("Saler")]
    public class ordersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<applicationUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ordersController(ApplicationDbContext context, UserManager<applicationUser> usermanagement, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _usermanagement = usermanagement;
            _httpContextAccessor = httpContextAccessor; 
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }

        // GET: Saler/orders
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var applicationDbContext = await _context.orders.Include(o => o.applicationUser).Include(o => o.status).ToListAsync();
            duyetDon a = new duyetDon();
            foreach (var item in applicationDbContext)
            {
                var orderDetail = await _context.orderDetails.Include(x => x.product).ThenInclude(a=>a.ApplicationUsers).Where(x => x.Id == item.Id).ToListAsync();
                foreach (var ite in orderDetail)
                {
                    a.orderDetail = ite;
                }

                a.orderList.Add(item);
            }

            a.userId = userId;

            return View(a);
        }
        [HttpPost]
        public async Task<IActionResult> Index(duyetDon duyetDon)
        {
            var order = await _context.orders.FindAsync(duyetDon.Id);
            order.IsDelete = true;
            order.orderStatusId = 2;
            _context.orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("Index", "orders");
        }


        // GET: Saler/orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.orders == null)
            {
                return NotFound();
            }

            var order = await _context.orders
                .Include(o => o.applicationUser)
                .Include(o => o.status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Saler/orders/Create
        public IActionResult Create()
        {
            ViewData["applicationUserId"] = new SelectList(_context.applicationUsers, "Id", "Id");
            ViewData["orderStatusId"] = new SelectList(_context.orderStatus, "Id", "Id");
            return View();
        }

        // POST: Saler/orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,createDate,IsDelete,orderStatusId,applicationUserId")] order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["applicationUserId"] = new SelectList(_context.applicationUsers, "Id", "Id", order.applicationUserId);
            ViewData["orderStatusId"] = new SelectList(_context.orderStatus, "Id", "Id", order.orderStatusId);
            return View(order);
        }

        // GET: Saler/orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.orders == null)
            {
                return NotFound();
            }

            var order = await _context.orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["applicationUserId"] = new SelectList(_context.applicationUsers, "Id", "Id", order.applicationUserId);
            ViewData["orderStatusId"] = new SelectList(_context.orderStatus, "Id", "Id", order.orderStatusId);
            return View(order);
        }

        // POST: Saler/orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,createDate,IsDelete,orderStatusId,applicationUserId")] order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!orderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["applicationUserId"] = new SelectList(_context.applicationUsers, "Id", "Id", order.applicationUserId);
            ViewData["orderStatusId"] = new SelectList(_context.orderStatus, "Id", "Id", order.orderStatusId);
            return View(order);
        }

        // GET: Saler/orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.orders == null)
            {
                return NotFound();
            }

            var order = await _context.orders
                .Include(o => o.applicationUser)
                .Include(o => o.status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Saler/orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.orders'  is null.");
            }
            var order = await _context.orders.FindAsync(id);
            if (order != null)
            {
                _context.orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool orderExists(int id)
        {
          return (_context.orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
