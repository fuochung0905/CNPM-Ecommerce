﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Areas.Admin.Components
{
    public class variationViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public variationViewComponent(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public IViewComponentResult Invoke()
        {
           
            var categories = _context.categories.ToList();
            var variotions=_context.variation.ToList();
            foreach(var ca in categories)
            {
                foreach (var item in variotions)
                {
                    if(ca.Id == item.categoryId) {
                        ca.variations.Add(item);
                    }
                }
            }
            return View(categories);
        }
    }
}
