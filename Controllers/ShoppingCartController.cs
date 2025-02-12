﻿using Microsoft.AspNetCore.Mvc;
using GameDrop.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace GameDrop.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _shoppingCartService.GetCartItemsByUserIdAsync(userId);
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            try
            {
                var product = _shoppingCartService.GetProductById(productId);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Product not found!";
                    return RedirectToAction("Shop");
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    TempData["ErrorMessage"] = "User not found!";
                    return RedirectToAction("Index");
                }

                await _shoppingCartService.AddToCartAsync(product, quantity, userId);

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
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartItems = await _shoppingCartService.GetCartItemsByUserIdAsync(userId);
                var cartItem = cartItems.SingleOrDefault(item => item.Product.ProductId == productId);
                if (cartItem != null)
                {
                    _shoppingCartService.RemoveFromCart(productId, userId);
                    TempData["SuccessMessage"] = "Product removed from cart successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Item not found in cart.";
                }
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
        public async Task<IActionResult> UpdateCart(int productId, int quantity)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartItems = await _shoppingCartService.GetCartItemsByUserIdAsync(userId);
                var cartItem = cartItems.SingleOrDefault(item => item.Product.ProductId == productId);
                if (cartItem != null)
                {
                    _shoppingCartService.UpdateCart(productId, quantity, userId);
                    TempData["SuccessMessage"] = "Cart updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Item not found in cart.";
                }
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
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _shoppingCartService.ClearCartAsync(userId);
                TempData["SuccessMessage"] = "Cart cleared successfully!";
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _shoppingCartService.ProcessPurchase(userId);
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