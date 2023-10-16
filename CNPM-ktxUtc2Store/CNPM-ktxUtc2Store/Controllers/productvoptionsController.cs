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
    public class productvoptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public productvoptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: productvoptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.productvoption.Include(p => p.product).Include(p => p.variationoption);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: productvoptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.productvoption == null)
            {
                return NotFound();
            }

            var productvoption = await _context.productvoption
                .Include(p => p.product)
                .Include(p => p.variationoption)
                .FirstOrDefaultAsync(m => m.productId == id);
            if (productvoption == null)
            {
                return NotFound();
            }

            return View(productvoption);
        }

        // GET: productvoptions/Create
        public IActionResult Create()
        {
            ViewData["productId"] = new SelectList(_context.products, "Id", "productName");
            ViewData["variationoptionId"] = new SelectList(_context.variation_option, "Id", "Id");
            return View();
        }

        // POST: productvoptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("productId,variationoptionId")] productvoption productvoption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productvoption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["productId"] = new SelectList(_context.products, "Id", "productName", productvoption.productId);
            ViewData["variationoptionId"] = new SelectList(_context.variation_option, "Id", "Id", productvoption.variationoptionId);
            return View(productvoption);
        }

        // GET: productvoptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.productvoption == null)
            {
                return NotFound();
            }

            var productvoption = await _context.productvoption.FindAsync(id);
            if (productvoption == null)
            {
                return NotFound();
            }
            ViewData["productId"] = new SelectList(_context.products, "Id", "productName", productvoption.productId);
            ViewData["variationoptionId"] = new SelectList(_context.variation_option, "Id", "Id", productvoption.variationoptionId);
            return View(productvoption);
        }

        // POST: productvoptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("productId,variationoptionId")] productvoption productvoption)
        {
            if (id != productvoption.productId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productvoption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!productvoptionExists(productvoption.productId))
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
            ViewData["productId"] = new SelectList(_context.products, "Id", "productName", productvoption.productId);
            ViewData["variationoptionId"] = new SelectList(_context.variation_option, "Id", "Id", productvoption.variationoptionId);
            return View(productvoption);
        }

        // GET: productvoptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.productvoption == null)
            {
                return NotFound();
            }

            var productvoption = await _context.productvoption
                .Include(p => p.product)
                .Include(p => p.variationoption)
                .FirstOrDefaultAsync(m => m.productId == id);
            if (productvoption == null)
            {
                return NotFound();
            }

            return View(productvoption);
        }

        // POST: productvoptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.productvoption == null)
            {
                return Problem("Entity set 'ApplicationDbContext.productvoption'  is null.");
            }
            var productvoption = await _context.productvoption.FindAsync(id);
            if (productvoption != null)
            {
                _context.productvoption.Remove(productvoption);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool productvoptionExists(int id)
        {
          return (_context.productvoption?.Any(e => e.productId == id)).GetValueOrDefault();
        }
    }
}
