using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;
using GameDrop.Models;
using Microsoft.EntityFrameworkCore;

namespace GameDrop.Controllers
{
    public class ShopController : Controller
    {
        private readonly GameDropDBContext _context;

        public ShopController(GameDropDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            var products = await _context.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int totalProducts = await _context.Products.CountAsync();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;

            return View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
