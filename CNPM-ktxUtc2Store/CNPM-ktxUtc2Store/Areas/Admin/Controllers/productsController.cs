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

        public productsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: Admin/products
        public IActionResult Index(string name)
        {
            var product = from p in _context.products select p;
            if (!string.IsNullOrEmpty(name))
            {
                    product = product.Where(x => x.productName.Contains(name) );
            }
            return View(product);
        }
        // GET: Admin/products/Details/5
        public IActionResult Details(int? id)
        {

            var product = new product();
            product = _context.products.Find(id);
             
            var variation = (from v in _context.variation
                             join c in _context.categories
                             on v.categoryId equals c.Id
                             where(c.Id==product.categoryId)
                             select new variation
                             {
                                 Id = v.Id,
                                 name = v.name,
                                value=v.value,
                                category=c
                             }).ToList();
            return View(variation);
        }
        // GET: Admin/products/Create
        public IActionResult Create()
        {
            ViewData["categoryId"] = new SelectList(_context.categories, "Id", "categoryName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(product product)
        {
            string uniqueFileName = uploadImage(product);

                product.imageUrl= uniqueFileName;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
        }
        //Get Admin/product/AddVariation/5
        public  IActionResult AddVariation(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product =  _context.products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            var variation=_context.variation.Where(x=>x.categoryId==product.categoryId).ToList();
            var selectLists= new List<SelectListItem>();
            foreach(var item in variation)
            {
                selectLists.Add(new SelectListItem(item.value, item.Id.ToString()));
            }
            var vm = new productVaritionCreateView()
            {
                Items = selectLists
            };

            ViewData["variationId"] = new SelectList(_context.variation.Where(x=>x.categoryId==product.categoryId), "Id", "value");
            return View(vm);  
        }

        [HttpPost]
        public IActionResult AddVariation(int id, productVaritionCreateView vm)
        {
            var product = _context.products.Find(id);
            foreach(var item in vm.selectVariation)
            {
                product.ProductVariations.Add(new productVariation
                {
                    variationId = Convert.ToInt32(item)
                }); 
            }
            _context.products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("Create");  

           
              
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
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, product product)
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
            ViewData["categoryId"] = new SelectList(_context.categories, "Id", "Id", product.categoryId);
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
                string uploadFoder = Path.Combine(_webHostEnvironment.WebRootPath, "images/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.image.FileName;
                string filePath = Path.Combine(uploadFoder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
