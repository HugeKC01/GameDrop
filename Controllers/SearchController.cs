using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;
using System.Linq;

namespace GameDrop.Controllers
{
    public class SearchController : Controller
    {
        private readonly GameDropDBContext _db;

        public SearchController(GameDropDBContext db)
        {
            _db = db;
        }

        public IActionResult Index(string SearchItem, string sortOrder, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            var products = _db.Products.AsQueryable();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentSearch = SearchItem;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.CategoryId = categoryId;
            ViewBag.Categories = _db.Categories.ToList();

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
                    products = products.Where(p => p.CategoryId == categoryId.Value);
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

            return View("Index", products.ToList());
        }
    }
}
