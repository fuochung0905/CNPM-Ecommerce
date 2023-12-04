using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNPM_ktxUtc2Store.Data;
using CNPM_ktxUtc2Store.Models;

namespace CNPM_ktxUtc2Store.Controllers
{
    public class UserAdressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserAdressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserAdresses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.userAdresses.Include(u => u.adress).Include(u => u.applicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserAdresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.userAdresses == null)
            {
                return NotFound();
            }

            var userAdress = await _context.userAdresses
                .Include(u => u.adress)
                .Include(u => u.applicationUser)
                .FirstOrDefaultAsync(m => m.AdressId == id);
            if (userAdress == null)
            {
                return NotFound();
            }

            return View(userAdress);
        }

        // GET: UserAdresses/Create
        public IActionResult Create()
        {
            ViewData["AdressId"] = new SelectList(_context.adresses, "Id", "Id");
            ViewData["applicationUserId"] = new SelectList(_context.applicationUsers, "Id", "Id");
            return View();
        }

        // POST: UserAdresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdressId,applicationUserId")] UserAdress userAdress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAdress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdressId"] = new SelectList(_context.adresses, "Id", "Id", userAdress.AdressId);
            ViewData["applicationUserId"] = new SelectList(_context.applicationUsers, "Id", "Id", userAdress.applicationUserId);
            return View(userAdress);
        }

        // GET: UserAdresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.userAdresses == null)
            {
                return NotFound();
            }

            var userAdress = await _context.userAdresses.FindAsync(id);
            if (userAdress == null)
            {
                return NotFound();
            }
            ViewData["AdressId"] = new SelectList(_context.adresses, "Id", "Id", userAdress.AdressId);
            ViewData["applicationUserId"] = new SelectList(_context.applicationUsers, "Id", "Id", userAdress.applicationUserId);
            return View(userAdress);
        }

        // POST: UserAdresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdressId,applicationUserId")] UserAdress userAdress)
        {
            if (id != userAdress.AdressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAdress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAdressExists(userAdress.AdressId))
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
            ViewData["AdressId"] = new SelectList(_context.adresses, "Id", "Id", userAdress.AdressId);
            ViewData["applicationUserId"] = new SelectList(_context.applicationUsers, "Id", "Id", userAdress.applicationUserId);
            return View(userAdress);
        }

        // GET: UserAdresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.userAdresses == null)
            {
                return NotFound();
            }

            var userAdress = await _context.userAdresses
                .Include(u => u.adress)
                .Include(u => u.applicationUser)
                .FirstOrDefaultAsync(m => m.AdressId == id);
            if (userAdress == null)
            {
                return NotFound();
            }

            return View(userAdress);
        }

        // POST: UserAdresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.userAdresses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.userAdresses'  is null.");
            }
            var userAdress = await _context.userAdresses.FindAsync(id);
            if (userAdress != null)
            {
                _context.userAdresses.Remove(userAdress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAdressExists(int id)
        {
          return (_context.userAdresses?.Any(e => e.AdressId == id)).GetValueOrDefault();
        }
    }
}
