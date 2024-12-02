using GameDrop.Data;
using GameDrop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GameDrop.Services
{
    public class ShoppingCartService
    {
        private readonly GameDropDBContext _db;

        public ShoppingCartService(GameDropDBContext db)
        {
            _db = db;
        }

        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(string userId)
        {
            return await _db.CartItems
                .Where(ci => ci.UserId == userId)
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        public void AddToCart(GameDrop_Product product, int quantity, string userId)
        {
            // Get the existing cart item for the product, if it exists
            var cartItem = _db.CartItems.SingleOrDefault(item => item.Product.ProductId == product.ProductId);

            // Calculate the total quantity after adding the requested amount
            var currentCartQuantity = cartItem?.Quantity ?? 0;
            var totalQuantity = currentCartQuantity + quantity;

            // Check if the total quantity exceeds the product's available stock
            if (totalQuantity > product.Quantity)
            {
                throw new InvalidOperationException("The quantity exceeds the available stock!");
            }

            // If the cart item doesn't exist, create a new one
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    UserId = userId
                };
                _db.CartItems.Add(cartItem);
            }
            else
            {
                // Update the quantity if within limits
                cartItem.Quantity = totalQuantity;
            }

            // Save changes to the database
            _db.SaveChanges();
        }


        public void RemoveFromCart(int productId, string userId)
        {
            var cartItem = _db.CartItems.SingleOrDefault(item => item.Product.ProductId == productId && item.UserId == userId);
            if (cartItem != null)
            {
                _db.CartItems.Remove(cartItem);
                _db.SaveChanges();
            }
        }

        public async Task<IActionResult> ClearCartAsync(string userId)
        {
            var cartItems = _db.CartItems.Where(ci => ci.UserId == userId);
            _db.CartItems.RemoveRange(cartItems);
            await _db.SaveChangesAsync();
            return new OkResult();
        }

        public void UpdateCart(int productId, int quantity, string userId)
        {
            var product = _db.Products.SingleOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found!");
            }

            if (quantity > product.Quantity)
            {
                throw new InvalidOperationException("The requested quantity exceeds the available stock!");
            }

            var cartItem = _db.CartItems.SingleOrDefault(item => item.Product.ProductId == productId && item.UserId == userId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                _db.SaveChanges();
            }
        }

        public void DeductStock(int productId, int quantity)
        {
            var product = _db.Products.SingleOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found!");
            }

            if (product.Quantity < quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product: {product.ProductName}");
            }

            product.Quantity -= quantity;
            _db.SaveChanges();
        }
        public void ProcessPurchase(string userId)
        {
            var cartItems = GetCartItemsByUserId(userId);
            foreach (var item in cartItems)
            {
                if (item.Quantity > item.Product.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product: {item.Product.ProductName}");
                }
            }

            // Deduct stock
            foreach (var item in cartItems)
            {
                DeductStock(item.Product.ProductId, item.Quantity);
            }

            // Clear the cart after purchase
            _db.CartItems.RemoveRange(_db.CartItems);
            _db.SaveChanges();
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            return await _db.CartItems
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        public async Task AddToCartAsync(GameDrop_Product product, int quantity, string userId)
        {
            var cartItem = await _db.CartItems
                .SingleOrDefaultAsync(item => item.Product.ProductId == product.ProductId && item.UserId == userId);

            int currentCartQuantity = cartItem?.Quantity ?? 0;
            int totalQuantity = currentCartQuantity + quantity;

            if (totalQuantity > product.Quantity)
            {
                throw new InvalidOperationException("The quantity exceeds the available stock!");
            }

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    UserId = userId
                };
                await _db.CartItems.AddAsync(cartItem);
            }
            else
            {
                cartItem.Quantity = totalQuantity;
            }

            await _db.SaveChangesAsync();
        }

        public List<CartItem> GetCartItemsByUserId(string userId)
        {
            return _db.CartItems.Include(ci => ci.Product).Where(ci => ci.UserId == userId).ToList();
        }

        public GameDrop_Product GetProductById(int productId) => _db.Products.SingleOrDefault(p => p.ProductId == productId);

        public int GetCartQuantityForProduct(int productId)
        {

            var cartItems = _db.CartItems.Include(ci => ci.Product).ToList();
            var productInCart = _db.CartItems.FirstOrDefault(item => item.Product.ProductId == productId);

            return productInCart != null ? productInCart.Quantity : 0;
        }
    }


    public class CartItem
    {
        public int Id { get; set; }
        public GameDrop_Product Product { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
    }
}