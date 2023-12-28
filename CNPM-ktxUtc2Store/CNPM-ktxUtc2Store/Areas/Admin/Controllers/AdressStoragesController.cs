using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNPM_ktxUtc2Store.Data;
using CNPM_ktxUtc2Store.Models;
using Microsoft.AspNetCore.Authorization;


namespace CNPM_ktxUtc2Store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
  
    public class AdressStoragesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdressStoragesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdressStorages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AdressStorage.Include(a => a.InforStorage);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/AdressStorages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AdressStorage == null)
            {
                return NotFound();
            }

            var adressStorage = await _context.AdressStorage
                .Include(a => a.InforStorage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adressStorage == null)
            {
                return NotFound();
            }

            return View(adressStorage);
        }

        // GET: Admin/AdressStorages/Create
        public IActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( AdressStorage adressStorage)
        {
            var infor = await _context.InforStorage.ToListAsync();
            foreach(var item in infor)
            {
                adressStorage.InforStorage = item;
            }
              
                _context.Add(adressStorage);
                await _context.SaveChangesAsync();
            
            
          
            return View(adressStorage);
        }

        // GET: Admin/AdressStorages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AdressStorage == null)
            {
                return NotFound();
            }

            var adressStorage = await _context.AdressStorage.FindAsync(id);
            if (adressStorage == null)
            {
                return NotFound();
            }
            ViewData["InforStorageId"] = new SelectList(_context.InforStorage, "Id", "Id", adressStorage.InforStorageId);
            return View(adressStorage);
        }

        // POST: Admin/AdressStorages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameAdress,InforStorageId")] AdressStorage adressStorage)
        {
            if (id != adressStorage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adressStorage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdressStorageExists(adressStorage.Id))
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
            ViewData["InforStorageId"] = new SelectList(_context.InforStorage, "Id", "Id", adressStorage.InforStorageId);
            return View(adressStorage);
        }

        // GET: Admin/AdressStorages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AdressStorage == null)
            {
                return NotFound();
            }

            var adressStorage = await _context.AdressStorage
                .Include(a => a.InforStorage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adressStorage == null)
            {
                return NotFound();
            }

            return View(adressStorage);
        }

        // POST: Admin/AdressStorages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdressStorage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AdressStorage'  is null.");
            }
            var adressStorage = await _context.AdressStorage.FindAsync(id);
            if (adressStorage != null)
            {
                _context.AdressStorage.Remove(adressStorage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdressStorageExists(int id)
        {
          return (_context.AdressStorage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
