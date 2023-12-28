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
using CNPM_ktxUtc2Store.Areas.Admin.dto;
using System.Net.WebSockets;

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
            productDto productDto = new productDto();
            var product = from p in _context.products.Where(x=>x.qty_inStock>0) select p;
           
            foreach(var item in product)
            {
                
                productDto.products.Add(item);
                var productvariation = _context.productVariations.Include(x=>x.variation).Where(x => x.productId == item.Id).ToList();
                foreach (var i in productvariation)
                {
                    productDto.productVariation.Add(i);
                }
            }
            if (!string.IsNullOrEmpty(name))
            {
                    product = product.Where(x => x.productName.Contains(name) );
                foreach (var item in product)
                {
                    productDto.products.Add(item);
                }

            }
            return View(productDto);
        }
        // GET: Admin/products/Details/5
        public IActionResult Details(int? id)
        {

            var product = new product();
            product = _context.products.Find(id);
             
            var variation = (from v in _context.variation
                             join c in _context.categories
                             on v.categoryId equals c.Id
                             where(c.Id==product.categoryId )
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
            if (product.oldprice.HasValue && product.price.HasValue)
            {
                if (product.oldprice.Value > product.price.Value)
                {
                    ModelState.AddModelError("", " Giá nhập nên bé hơn giá bán");
                }
            }
            if (ModelState.IsValid)
            {
                string uni=uploadImage(product);
                product.imageUrl = uni;
                product.soluongnhap = product.qty_inStock;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoryId"] = new SelectList(_context.categories, "Id", "Id", product.categoryId);
            return View(product);

        }
        //Get Admin/product/AddVariation/5
        public async Task<IActionResult> AddVariation(int? id)
        {
            var product =await _context.products.FindAsync(id);
            var variation =await _context.variation.Where(x => x.categoryId == product.categoryId).ToListAsync();
            
            productvariationDto dto = new productvariationDto();
            dto.productId = product.Id;
            
            foreach (var item in variation)
            {
                dto.Variations.Add(item);
            }
            var productvariation =await _context.productVariations.Where(x => x.productId == product.Id).ToListAsync();
            foreach (var pv in productvariation)
            {
                dto.productVariations.Add(pv);
            }
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> AddVariation(int id, productvariationDto productvariationDto)
        {
            var product= await _context.products.FindAsync(productvariationDto.productId);
      
            var variation =await _context.variation.FindAsync(productvariationDto.Id);
            productVariation model = new productVariation();
            model.product = product;
            model.variation = variation;
            _context.productVariations.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("AddVariation", "products");
        }
        public async Task<IActionResult> DeleteVariation(int productId, int variationId)
        {
        
                var productVariation = await _context.productVariations
                    .Where(x => x.productId == productId && x.variationId == variationId)
                    .FirstOrDefaultAsync();

              if (productVariation != null)
            {
                _context.productVariations.Remove(productVariation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            var pr = _context.products.Find(id);
            pr.productName = product.productName;
            pr.price = product.price;   
            pr.description=product.description;
            pr.oldprice=product.oldprice;
            pr.qty_inStock=product.qty_inStock;
                    _context.products.Update(pr);
                    await _context.SaveChangesAsync();
          
            return RedirectToAction("Index","products");
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
            var productvari=await _context.productVariations.Where(x=>x.productId==id).ToListAsync();
            foreach (var variation in productvari)
            {
                _context.productVariations.Remove(variation);
               await _context.SaveChangesAsync();
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
