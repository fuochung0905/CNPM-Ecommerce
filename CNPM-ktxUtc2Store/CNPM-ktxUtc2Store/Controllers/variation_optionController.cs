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
    public class variation_optionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public variation_optionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: variation_option
        public IActionResult Index()
        {


            var variation = (from v in _context.variation
                             join c in _context.categories
                             on v.categoryId equals c.Id
                             select new variation
                             {
                                 Id = v.Id,
                                 name = v.name,
                                 category = c
                             });

            var variation_option= new List<variation_option>();
            variation_option = (from vo in _context.variation_option
                                join v in variation
                                on vo.variationId equals v.Id
                                select new variation_option
                                {
                                    Id=vo.Id,
                                    value = vo.value,   
                                    variation=v
                                }).ToList();
                              
            return View(variation_option);
        }

        // GET: variation_option/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.variation_option == null)
            {
                return NotFound();
            }

            var variation_option = await _context.variation_option
                .Include(v => v.variation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variation_option == null)
            {
                return NotFound();
            }

            return View(variation_option);
        }

        // GET: variation_option/Create
        public IActionResult Create()
        {
            ViewData["variationId"] = new SelectList(_context.variation, "Id", "name");
            return View();
        }

        // POST: variation_option/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( variation_option variation_option)
        {
           
                _context.Add(variation_option);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
        }

        // GET: variation_option/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.variation_option == null)
            {
                return NotFound();
            }

            var variation_option = await _context.variation_option.FindAsync(id);
            if (variation_option == null)
            {
                return NotFound();
            }
            ViewData["variationId"] = new SelectList(_context.variation, "Id", "name", variation_option.variationId);
            return View(variation_option);
        }

        // POST: variation_option/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  variation_option variation_option)
        {
            if (id != variation_option.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variation_option);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!variation_optionExists(variation_option.Id))
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
            ViewData["variationId"] = new SelectList(_context.variation, "Id", "Id", variation_option.variationId);
            return View(variation_option);
        }

        // GET: variation_option/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.variation_option == null)
            {
                return NotFound();
            }

            var variation_option = await _context.variation_option
                .Include(v => v.variation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variation_option == null)
            {
                return NotFound();
            }

            return View(variation_option);
        }

        // POST: variation_option/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.variation_option == null)
            {
                return Problem("Entity set 'ApplicationDbContext.variation_option'  is null.");
            }
            var variation_option = await _context.variation_option.FindAsync(id);
            if (variation_option != null)
            {
                _context.variation_option.Remove(variation_option);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool variation_optionExists(int id)
        {
          return (_context.variation_option?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
