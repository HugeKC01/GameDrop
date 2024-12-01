using Microsoft.AspNetCore.Mvc;
using GameDrop.Services;

namespace GameDrop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var cartItems = _shoppingCartService.GetCartItems();
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            try
            {
                var product = _shoppingCartService.GetProductById(productId);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Product not found!";
                    return RedirectToAction("Shop");
                }

                _shoppingCartService.AddToCart(product, quantity);

                TempData["SuccessMessage"] = "Product added to cart successfully!";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Details", "Shop", new { id = productId });
            }
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            try
            {
                _shoppingCartService.RemoveFromCart(productId);
                TempData["SuccessMessage"] = "Product removed from cart successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            try
            {
                var product = _shoppingCartService.GetProductById(productId);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Product not found!";
                    return RedirectToAction("Index");
                }

                _shoppingCartService.UpdateCart(productId, quantity);
                TempData["SuccessMessage"] = "Cart updated successfully!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Buy()
        {
            try
            {
                _shoppingCartService.ProcessPurchase();
                TempData["SuccessMessage"] = "Purchase completed successfully!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
        public IActionResult ProductDetails(int id)
        {
            // Fetch the product by ID
            var product = _shoppingCartService.GetProductById(id);
            if (product == null)
            {
                return NotFound(); // Return 404 if the product doesn't exist
            }

            // Fetch the quantity of the product currently in the cart
            int cartQuantity = _shoppingCartService.GetCartQuantityForProduct(id);

            // Pass the cart quantity to the view using ViewData
            ViewData["CartQuantity"] = cartQuantity;

            // Return the product details to the view
            return View(product);
        }


    }
}
