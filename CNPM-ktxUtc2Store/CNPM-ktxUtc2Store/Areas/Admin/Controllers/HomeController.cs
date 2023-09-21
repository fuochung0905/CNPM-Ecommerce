﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CNPM_ktxUtc2Store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller

    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
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
                listProduct = _context.products.Where(n => n.productName.Contains(SearchString)).ToList();
            }
            else
            {
                listProduct = _context.products.ToList();
            }
            ViewBag.currentFilter = SearchString;
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            listProduct = listProduct.OrderByDescending(n => n.Id).ToList();
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }
    }
}
