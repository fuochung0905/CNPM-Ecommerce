using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNPM_ktxUtc2Store.Data;
using CNPM_ktxUtc2Store.Models;
using Microsoft.AspNetCore.Identity;

namespace CNPM_ktxUtc2Store.Controllers
{
    public class AdressesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<applicationUser> _usermanagement;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdressesController(ApplicationDbContext context, UserManager<applicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _usermanagement= userManager;
            _httpContextAccessor= httpContextAccessor;
        }
        private string GetUserId()
        {

            var pricipal = _httpContextAccessor.HttpContext.User;
            string userId = _usermanagement.GetUserId(pricipal);

            return userId;
        }

        // GET: Adresses
        public async Task<IActionResult> Index()
        {
            var adress =await _context.adresses.ToListAsync();
            changeAdress ca = new changeAdress();
            foreach( var item in adress)
            {
                ca.orderList.Add(item);
            }
            return View(ca);
        }
        [HttpPost]
        public async Task<IActionResult> Index(listAdressUser changeadress)
        {
           
            var userId = GetUserId();
            var useradress = await _context.userAdresses
                   .Include(x => x.adress)
                   .Include(x => x.applicationUser)
                   .Where(x => x.applicationUserId == userId).ToListAsync();
            foreach (var item in useradress)
            {
                item.isDefine = false;
                _context.userAdresses.Update(item);
                _context.SaveChanges();
            }
            var result = await _context.userAdresses
                  .Where(x => x.applicationUserId == userId)
                  .Where(x => x.AdressId == changeadress.Id)
                  .ToListAsync();


            foreach (var item in result)
            {
                item.isDefine = true;
                _context.userAdresses.Update(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Userorder", "UserOrder");

        }


        // GET: Adresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.adresses == null)
            {
                return NotFound();
            }

            var adress = await _context.adresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adress == null)
            {
                return NotFound();
            }

            return View(adress);
        }

        // GET: Adresses/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,homeAdress,villageAdress,districAdress")] Adress adress)
        {
            if (ModelState.IsValid)
            {
                var userID = GetUserId();
                _context.Add(adress);
                await _context.SaveChangesAsync();
                var user = _context.applicationUsers.Find(userID);
                var diachi = from dc in _context.adresses where (dc.homeAdress == adress.homeAdress && dc.villageAdress == adress.villageAdress && dc.districAdress == adress.districAdress) select dc;
               var useradress= _context.userAdresses
                    .Include(x=>x.adress)
                    .Include(x=>x.applicationUser)
                    .Where(x=>x.applicationUserId==userID).ToList();
                foreach( var item in useradress)
                {
                    item.isDefine=false;
                    _context.userAdresses.Update(item);
                    _context.SaveChanges();
                }
              
                UserAdress ua = new UserAdress();
                ua.applicationUser = user;
                foreach(var item in diachi)
                {
                    ua.adress = item;
                }
                ua.isDefine = true;
                _context.userAdresses.Add(ua);
                _context.SaveChanges();
                
                return RedirectToAction("Userorder", "UserOrder");
            }
            return View(adress);
        }

        // GET: Adresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.adresses == null)
            {
                return NotFound();
            }

            var adress = await _context.adresses.FindAsync(id);
            if (adress == null)
            {
                return NotFound();
            }
            return View(adress);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,homeAdress,villageAdress,districAdress")] Adress adress)
        {
            if (id != adress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdressExists(adress.Id))
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
            return View(adress);
        }

        // GET: Adresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.adresses == null)
            {
                return NotFound();
            }

            var adress = await _context.adresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adress == null)
            {
                return NotFound();
            }

            return View(adress);
        }

        // POST: Adresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.adresses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.adresses'  is null.");
            }
            var adress = await _context.adresses.FindAsync(id);
            if (adress != null)
            {
                _context.adresses.Remove(adress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdressExists(int id)
        {
          return (_context.adresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
