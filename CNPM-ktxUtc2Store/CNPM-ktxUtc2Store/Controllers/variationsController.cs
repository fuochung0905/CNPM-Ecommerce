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

    public class variationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public variationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: variations
        public  IActionResult Index()
        {
            var variation = new List<variation>();
            variation =  (from v in _context.variation
                         join c in _context.categories
                         on v.categoryId equals c.Id
                         select new variation
                         {
                             Id = v.Id,
                             name = v.name,
                             category = c
                         }).ToList();
            return View(variation);
        }

        // GET: variations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.variation == null)
            {
                return NotFound();
            }

            var variation = await _context.variation
                .Include(v => v.category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variation == null)
            {
                return NotFound();
            }

            return View(variation);
        }

        // GET: variations/Create
        public IActionResult Create()
        {
            ViewData["categoryId"] = new SelectList(_context.categories, "Id", "categoryName");
            return View();
        }

        // POST: variations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( variation variation)
        {
           
                _context.Add(variation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
        }

        // GET: variations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.variation == null)
            {
                return NotFound();
            }

            var variation = await _context.variation.FindAsync(id);
            if (variation == null)
            {
                return NotFound();
            }
            ViewData["categoryId"] = new SelectList(_context.categories, "Id", "Id", variation.categoryId);
            return View(variation);
        }

        // POST: variations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  variation variation)
        {
            if (id != variation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!variationExists(variation.Id))
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
            ViewData["categoryId"] = new SelectList(_context.categories, "Id", "categoryName", variation.categoryId);
            return View(variation);
        }

        // GET: variations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.variation == null)
            {
                return NotFound();
            }

            var variation = await _context.variation
                .Include(v => v.category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variation == null)
            {
                return NotFound();
            }

            return View(variation);
        }

        // POST: variations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.variation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.variation'  is null.");
            }
            var variation = await _context.variation.FindAsync(id);
            if (variation != null)
            {
                _context.variation.Remove(variation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool variationExists(int id)
        {
          return (_context.variation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
