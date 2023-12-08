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
    public class InforStoragesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InforStoragesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/InforStorages
        public async Task<IActionResult> Index()
        {
              return _context.InforStorage != null ? 
                          View(await _context.InforStorage.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.InforStorage'  is null.");
        }

        // GET: Admin/InforStorages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InforStorage == null)
            {
                return NotFound();
            }

            var inforStorage = await _context.InforStorage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inforStorage == null)
            {
                return NotFound();
            }

            return View(inforStorage);
        }

        // GET: Admin/InforStorages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/InforStorages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,namestorage,logo,linkfacbook,linkInstagram,linkyoutube,linktiktok")] InforStorage inforStorage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inforStorage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inforStorage);
        }

        // GET: Admin/InforStorages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InforStorage == null)
            {
                return NotFound();
            }

            var inforStorage = await _context.InforStorage.FindAsync(id);
            if (inforStorage == null)
            {
                return NotFound();
            }
            return View(inforStorage);
        }

        // POST: Admin/InforStorages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,namestorage,logo,linkfacbook,linkInstagram,linkyoutube,linktiktok")] InforStorage inforStorage)
        {
            if (id != inforStorage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inforStorage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InforStorageExists(inforStorage.Id))
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
            return View(inforStorage);
        }

        // GET: Admin/InforStorages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InforStorage == null)
            {
                return NotFound();
            }

            var inforStorage = await _context.InforStorage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inforStorage == null)
            {
                return NotFound();
            }

            return View(inforStorage);
        }

        // POST: Admin/InforStorages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InforStorage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.InforStorage'  is null.");
            }
            var inforStorage = await _context.InforStorage.FindAsync(id);
            if (inforStorage != null)
            {
                _context.InforStorage.Remove(inforStorage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InforStorageExists(int id)
        {
          return (_context.InforStorage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
