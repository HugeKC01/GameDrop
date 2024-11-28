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

        public IActionResult Index(string SearchItem, string sortOrder)
        {
            var products = _db.Products.AsQueryable();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentSearch = SearchItem;

            if (!string.IsNullOrEmpty(SearchItem))
            {
                products = products.Where(ProductSearch =>
                    ProductSearch.ProductId.ToString().Contains(SearchItem) ||
                    (ProductSearch.ProductName != null && ProductSearch.ProductName.Contains(SearchItem)) ||
                    (ProductSearch.ProductDescription != null && ProductSearch.ProductDescription.Contains(SearchItem))
                );
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
