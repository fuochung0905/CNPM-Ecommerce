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

using X.PagedList;

namespace CNPM_ktxUtc2Store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class productsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public productsController(ApplicationDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;
        }

        // GET: Admin/products
        public IActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var listProduct = new List<product>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                listProduct = _context.products.Where(n=>n.productName.Contains(SearchString)).ToList();
            }
            else
            {
                listProduct =_context.products.ToList();
            }
            ViewBag.currentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            listProduct = listProduct.OrderByDescending(n => n.Id).ToList();
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/products/Create
        public IActionResult Create()
        {
            ViewData["categoryId"] = new SelectList(_context.categories, "categoryName", "Id");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqueFileName = uploadImage(product);
                    var data = new product
                    {
                        productName=product.productName,
                        description=product.description,
                        discount=product.discount,
                        price=product.price,
                        imageUrl=uniqueFileName,
                        categoryId=product.categoryId,
                    };
                    _context.Add(data);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Record Successfully saved";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

          
            ViewData["categoryId"] = new SelectList(_context.categories, "categoryName", "Id", product.categoryId);
            return View(product);
        }

        // GET: Admin/products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["categoryId"] = new SelectList(_context.categories, "categoryName", "Id", product.categoryId);
            return View(product);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,productName,description,discount,price,imageUrl,categoryId,categoryName")] product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!productExists(product.Id))
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
            ViewData["categoryId"] = new SelectList(_context.categories, "categoryName", "Id", product.categoryId);
            return View(product);
        }

        // GET: Admin/products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.products'  is null.");
            }
            var product = await _context.products.FindAsync(id);
            if (product != null)
            {
                _context.products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool productExists(int id)
        {
          return (_context.products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private string uploadImage(product model)
        {
            string uniqueFileName = string.Empty;
            if (model.image != null)
            {
                string uploadFoder = Path.Combine(_webHostEnvironment.WebRootPath, ("images/"));
                  uniqueFileName = Guid.NewGuid().ToString() + "_" + model.image.FileName;
                string filePath=Path.Combine(uploadFoder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.image.CopyTo(fileStream); 
                }
            }
            return uniqueFileName;
        }
    }
   
}
