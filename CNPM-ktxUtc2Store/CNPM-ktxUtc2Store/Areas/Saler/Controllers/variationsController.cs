using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNPM_ktxUtc2Store.Data;
using CNPM_ktxUtc2Store.Models;

namespace CNPM_ktxUtc2Store.Areas.Saler.Controllers
{
    [Area("Saler")]
    public class variationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public variationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Saler/variations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.variation.Include(v => v.category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Saler/variations/Details/5
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

        // GET: Saler/variations/Create
        public IActionResult Create()
        {
            ViewData["categoryId"] = new SelectList(_context.categories, "Id", "Id");
            return View();
        }

        // POST: Saler/variations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,value,categoryId")] variation variation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(variation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoryId"] = new SelectList(_context.categories, "Id", "Id", variation.categoryId);
            return View(variation);
        }

        // GET: Saler/variations/Edit/5
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

        // POST: Saler/variations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,value,categoryId")] variation variation)
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
            ViewData["categoryId"] = new SelectList(_context.categories, "Id", "Id", variation.categoryId);
            return View(variation);
        }

        // GET: Saler/variations/Delete/5
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

        // POST: Saler/variations/Delete/5
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
