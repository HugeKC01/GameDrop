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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
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
