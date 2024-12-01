using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;
using Microsoft.EntityFrameworkCore;
using GameDrop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameDrop.Services;

namespace GameDrop.Controllers
{
    //[Authorize]
    public class OrderController : Controller
    {
        private readonly GameDropDBContext _db;
        private readonly ShoppingCartService _shoppingCartService;
        public OrderController(GameDropDBContext db,ShoppingCartService shoppingCartService)
        {
            _db = db;
            _shoppingCartService = shoppingCartService;
        }
        // GET: Order details from the shop page
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ShoppingCart()
        {
            var cartItems = await GetShoppingCartItemsAsync();
            if (!cartItems.Any())
            {
                ViewBag.ErrorMessage = "Your shopping cart is empty.";
            }

            return View(cartItems);
        }

        private async Task AddShoppingCartItemsToOrderDetails(int orderId)
        {
            var cartItems = await GetShoppingCartItemsAsync();
            foreach (var item in cartItems)
            {
                var orderDetail = new GameDrop_OrderDetails
                {
                    OrderId = orderId,
                    ProductId = item.Product.ProductId,
                    OrderProductName = item.Product.ProductName,
                    OrderQuantity = item.Quantity,
                    Total = item.Product.ProductPrice * item.Quantity
                };
                _db.OrderDetails.Add(orderDetail);
            }
            await _db.SaveChangesAsync();
        }

        private async Task<List<CartItem>> GetShoppingCartItemsAsync()
        {
            // Assuming you have a way to identify the current user or session
            // For example, using User.Identity.Name or a session ID
            var userId = User?.Identity?.Name;
            if (userId == null)
            {
                return new List<CartItem>();
            }
            return await _db.CartItems
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        //GET: Order details view
        public IActionResult OrderDetails(int id)
        {
            var cartItems = _shoppingCartService.GetCartItems();
            return View(cartItems);
        }

        // Delete order details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrderDetail(int orderDetailId)
        {
            var orderDetail = await _db.OrderDetails.FindAsync(orderDetailId);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _db.OrderDetails.Remove(orderDetail);
            await _db.SaveChangesAsync();

            return RedirectToAction("OrderDetails", new { id = orderDetail.OrderId });
        }

        // Proceed to payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProceedToPayment(int orderId)
        {
            await AddShoppingCartItemsToOrderDetails(orderId);
            return RedirectToAction("Payment", new { id = orderId });
        }

        public IActionResult Payment(int id)
        {
            // Implement your payment view logic here
            ViewBag.OrderId = id;
            return View();

        }
        // complete payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompletePayment(int orderId)
        {
            // Implement your payment completion logic here
            // For now, just mark the order as completed and redirect to a confirmation page

            var order = _db.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = "Completed";
            _db.SaveChanges();

            return RedirectToAction("PaymentConfirmation", new { id = orderId });
        }

        public IActionResult PaymentConfirmation(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}