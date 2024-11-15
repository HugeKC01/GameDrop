using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;

namespace GameDrop.Controllers
{
    public class SearchController : Controller
    {
        private readonly GameDropDBContext _db;

        public SearchController(GameDropDBContext db)
        {
            _db = db;
        }

        public IActionResult Index(string SearchItem)
        {
            if (!string.IsNullOrEmpty(SearchItem))
            {
                return View("Index", _db.Products.ToList());
            }
            else
            {
                var searchResult = _db.Products.Where(ProductSearch =>
                    ProductSearch.ProductId.ToString().Contains(SearchItem) ||
                    (ProductSearch.ProductName != null && ProductSearch.ProductName.Contains(SearchItem)) ||
                    (ProductSearch.ProductDescription != null && ProductSearch.ProductDescription.Contains(SearchItem)) ||
                    (ProductSearch.ProductPrice != null && ProductSearch.ProductPrice.ToString().Equals(SearchItem))
                ).ToList();
            }
            return View("Index", searchResults);
        }
    }
}
