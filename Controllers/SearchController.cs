﻿using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;
using System.Linq;
using GameDrop.Services;
using iTextSharp.text;

namespace GameDrop.Controllers
{
    public class SearchController : Controller
    {
        private readonly GameDropDBContext _db;
        private readonly CategoryService _categoryService;
        private const int PageSize = 10;

        public SearchController(GameDropDBContext db, CategoryService categoryService)
        {
            _db = db;
            _categoryService = categoryService;
        }

        public IActionResult Index(string SearchItem, string sortOrder, decimal? minPrice, decimal? maxPrice, int? categoryId, int pageNumber = 1)
        {
            var products = _db.Products.AsQueryable();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentSearch = SearchItem;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.Categories = _categoryService.GetCategories();
            ViewBag.CategoryId = categoryId;

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice <= maxPrice.Value);
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(SearchItem))
            {
                products = products.Where(ProductSearch =>
                    ProductSearch.ProductId.ToString().Contains(SearchItem) ||
                    (ProductSearch.ProductName != null && ProductSearch.ProductName.Contains(SearchItem)) ||
                    (ProductSearch.ProductDescription != null && ProductSearch.ProductDescription.Contains(SearchItem))
                );
                ViewData["Title"] = "Search Results: " + SearchItem;
            }
            else
            {
                if (categoryId.HasValue)
                {
                    var category = _db.Categories.FirstOrDefault(c => c.CategoryId == categoryId.Value);
                    ViewData["Title"] = category != null ? category.CategoryName : "Search List";
                }
                else
                {
                    ViewData["Title"] = "All Products";
                }
            }

            products = sortOrder switch
            {
                "name" => products.OrderBy(p => p.ProductName),
                "name_desc" => products.OrderByDescending(p => p.ProductName),
                "price" => products.OrderBy(p => p.ProductPrice),
                "price_desc" => products.OrderByDescending(p => p.ProductPrice),
                _ => products.OrderBy(p => p.ProductName),
            };

            var totalItems = products.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            products = products.Skip((pageNumber - 1) * PageSize).Take(PageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;

            return View("Index", products.ToList());
        }
    }
}
