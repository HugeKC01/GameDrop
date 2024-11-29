using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameDrop.Models;
using GameDrop.Data;

namespace GameDrop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly GameDropDBContext _db;

        public ShoppingCartController(GameDropDBContext db)
        {
            _db = db;
        }

        // Display shopping cart
        public async Task<IActionResult> Index()
        {
            var shoppingCartItems = await _db.ShoppingCarts
                .Include(c => c.Product)
                .ToListAsync();

            return View(shoppingCartItems);
        }

        // Add a product to the shopping cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = await _db.ShoppingCarts
                .FirstOrDefaultAsync(c => c.ProductId == productId);

            if (cartItem == null)
            {
                // Add new item to the cart
                cartItem = new GameDrop_ShoppingCart
                {
                    ProductId = productId,
                    Product = product,
                    ShoppingCartName = "DefaultCart",
                };
                _db.ShoppingCarts.Add(cartItem);
            }
            else
            {
                // Increase quantity
                product.Quantity += 1;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Increase product quantity
        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int cartItemId)
        {
            var cartItem = await _db.ShoppingCarts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.ShoppingCartId == cartItemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Product.Quantity += 1;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Decrease product quantity
        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int cartItemId)
        {
            var cartItem = await _db.ShoppingCarts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.ShoppingCartId == cartItemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            if (cartItem.Product.Quantity > 1)
            {
                cartItem.Product.Quantity -= 1;
            }
            else
            {
                // Optionally, remove the item if quantity drops to 0
                _db.ShoppingCarts.Remove(cartItem);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
