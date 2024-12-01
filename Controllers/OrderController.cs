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
        public OrderController(GameDropDBContext db)
        {
            _db = db;
        }
        // GET: Order details from the shop page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyNow(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            var product = await _db.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Create a new order
            var order = new GameDrop_Order
            {
                OrderDate = DateTime.Now.ToString("yyyy-MM-dd"),
                OrderStatus = "Pending",
                // Add other order details as needed
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Create order details for the product being bought now
            var orderDetails = new GameDrop_OrderDetails
            {
                OrderId = order.OrderId,
                ProductId = product.ProductId,
                OrderProductName = product.ProductName,
                OrderQuantity = quantity,
                Total = product.ProductPrice * quantity
            };

            _db.OrderDetails.Add(orderDetails);
            await _db.SaveChangesAsync();

            return RedirectToAction("OrderDetails", new { id = order.OrderId });
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
            var orderDetails = _db.OrderDetails
                .Where(od => od.OrderId == id)
                .ToList();

            if (!orderDetails.Any())
            {
                return NotFound();
            }

            foreach (var detail in orderDetails)
            {
                var product = _db.Products.FirstOrDefault(p => p.ProductId == detail.ProductId);
                if (product != null)
                {
                    var base64Image = Convert.ToBase64String(product.ProductImageData);
                    var imageType = product.ProductImageType;
                    ViewBag.ProductImages ??= new Dictionary<int, string>();
                    ViewBag.ProductImages[detail.ProductId] = $"data:{imageType};base64,{base64Image}";
                }
            }

            return View(orderDetails);
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
        public IActionResult ProceedToPayment(int orderId)
        {
            // Implement your payment logic here
            // For now, just redirect to a placeholder payment page
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