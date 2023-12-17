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
using CNPM_ktxUtc2Store.Migrations;

namespace CNPM_ktxUtc2Store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BannerStoragesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BannerStoragesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        private string uploadImage(BannerStorage model)
        {
            string uniqueFileName = string.Empty;
            if (model.picture != null)
            {
                string uploadFoder = Path.Combine(_webHostEnvironment.WebRootPath, "images/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.picture.FileName;
                string filePath = Path.Combine(uploadFoder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Admin/BannerStorages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BannerStorage.Include(b => b.InforStorage);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/BannerStorages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BannerStorage == null)
            {
                return NotFound();
            }

            var bannerStorage = await _context.BannerStorage
                .Include(b => b.InforStorage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bannerStorage == null)
            {
                return NotFound();
            }

            return View(bannerStorage);
        }

        // GET: Admin/BannerStorages/Create
        public IActionResult Create()
        {
       
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( BannerStorage bannerStorage)
        {
            if (ModelState.IsValid)
            {
                var infor = await _context.InforStorage.ToListAsync();
                foreach (var item in infor)
                {
                    bannerStorage.InforStorage = item;
                }
                string uniqueFileName = uploadImage(bannerStorage);
                bannerStorage.Bannerpicture = uniqueFileName;
                _context.Add(bannerStorage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InforStorageId"] = new SelectList(_context.InforStorage, "Id", "Id", bannerStorage.InforStorageId);
            return View(bannerStorage);
        }

        // GET: Admin/BannerStorages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BannerStorage == null)
            {
                return NotFound();
            }

            var bannerStorage = await _context.BannerStorage.FindAsync(id);
            if (bannerStorage == null)
            {
                return NotFound();
            }
            ViewData["InforStorageId"] = new SelectList(_context.InforStorage, "Id", "Id", bannerStorage.InforStorageId);
            return View(bannerStorage);
        }

        // POST: Admin/BannerStorages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Bannerpicture,InforStorageId")] BannerStorage bannerStorage)
        {
            if (id != bannerStorage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bannerStorage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerStorageExists(bannerStorage.Id))
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
            ViewData["InforStorageId"] = new SelectList(_context.InforStorage, "Id", "Id", bannerStorage.InforStorageId);
            return View(bannerStorage);
        }

        // GET: Admin/BannerStorages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BannerStorage == null)
            {
                return NotFound();
            }

            var bannerStorage = await _context.BannerStorage
                .Include(b => b.InforStorage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bannerStorage == null)
            {
                return NotFound();
            }

            return View(bannerStorage);
        }

        // POST: Admin/BannerStorages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BannerStorage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BannerStorage'  is null.");
            }
            var bannerStorage = await _context.BannerStorage.FindAsync(id);
            if (bannerStorage != null)
            {
                _context.BannerStorage.Remove(bannerStorage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerStorageExists(int id)
        {
          return (_context.BannerStorage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
