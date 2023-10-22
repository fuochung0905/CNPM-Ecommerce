using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNPM_ktxUtc2Store.Data;
using CNPM_ktxUtc2Store.Models;

namespace CNPM_ktxUtc2Store.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class productVariationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public productVariationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/productVariations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.productVariations.Include(p => p.product).Include(p => p.variation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/productVariations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.productVariations == null)
            {
                return NotFound();
            }

            var productVariation = await _context.productVariations
                .Include(p => p.product)
                .Include(p => p.variation)
                .FirstOrDefaultAsync(m => m.productId == id);
            if (productVariation == null)
            {
                return NotFound();
            }

            return View(productVariation);
        }

        // GET: Admin/productVariations/Create
        public IActionResult Create()
        {
            ViewData["productId"] = new SelectList(_context.products, "Id", "productName");
            ViewData["variationId"] = new SelectList(_context.variation, "Id", "Id");
            return View();
        }

        // POST: Admin/productVariations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("productId,variationId")] productVariation productVariation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productVariation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["productId"] = new SelectList(_context.products, "Id", "productName", productVariation.productId);
            ViewData["variationId"] = new SelectList(_context.variation, "Id", "Id", productVariation.variationId);
            return View(productVariation);
        }

        // GET: Admin/productVariations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.productVariations == null)
            {
                return NotFound();
            }

            var productVariation = await _context.productVariations.FindAsync(id);
            if (productVariation == null)
            {
                return NotFound();
            }
            ViewData["productId"] = new SelectList(_context.products, "Id", "productName", productVariation.productId);
            ViewData["variationId"] = new SelectList(_context.variation, "Id", "Id", productVariation.variationId);
            return View(productVariation);
        }

        // POST: Admin/productVariations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("productId,variationId")] productVariation productVariation)
        {
            if (id != productVariation.productId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productVariation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!productVariationExists(productVariation.productId))
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
            ViewData["productId"] = new SelectList(_context.products, "Id", "productName", productVariation.productId);
            ViewData["variationId"] = new SelectList(_context.variation, "Id", "Id", productVariation.variationId);
            return View(productVariation);
        }

        // GET: Admin/productVariations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.productVariations == null)
            {
                return NotFound();
            }

            var productVariation = await _context.productVariations
                .Include(p => p.product)
                .Include(p => p.variation)
                .FirstOrDefaultAsync(m => m.productId == id);
            if (productVariation == null)
            {
                return NotFound();
            }

            return View(productVariation);
        }

        // POST: Admin/productVariations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.productVariations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.productVariations'  is null.");
            }
            var productVariation = await _context.productVariations.FindAsync(id);
            if (productVariation != null)
            {
                _context.productVariations.Remove(productVariation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool productVariationExists(int id)
        {
          return (_context.productVariations?.Any(e => e.productId == id)).GetValueOrDefault();
        }
    }
}
