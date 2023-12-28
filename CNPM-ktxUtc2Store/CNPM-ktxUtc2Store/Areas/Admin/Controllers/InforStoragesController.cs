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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InforStoragesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        private string uploadImage(InforStorage  model)
        {
            string uniqueFileName = string.Empty;
            if (model.pictureLogo != null)
            {
                string uploadFoder = Path.Combine(_webHostEnvironment.WebRootPath, "images/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.pictureLogo.FileName;
                string filePath = Path.Combine(uploadFoder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.pictureLogo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( InforStorage inforStorage)
        {
          
                string uniqueFileName = uploadImage(inforStorage);
                inforStorage.logo = uniqueFileName;
                _context.Add(inforStorage);
                await _context.SaveChangesAsync();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  InforStorage inforStorage)
        {
            if (id != inforStorage.Id)
            {
                return NotFound();
            }
            var infor = await _context.InforStorage.ToListAsync();
            foreach (var item in infor)
            {
                item.namestorage = inforStorage.namestorage;
                item.phonenumbershop=inforStorage.phonenumbershop;
                item.emailwork = inforStorage.emailwork;
                item.emailcskh= inforStorage.emailcskh;
                item.timework= inforStorage.timework;
                item.linkfacbook=inforStorage.linkfacbook;
                item.linkInstagram=inforStorage.linkInstagram;
                item.linkyoutube = inforStorage.linkyoutube;
                item.linktiktok=inforStorage .linktiktok;
                _context.InforStorage.Update(item);
                await _context.SaveChangesAsync();
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
