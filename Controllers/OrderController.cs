using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;
using Microsoft.EntityFrameworkCore;
using GameDrop.Models;
using Microsoft.AspNetCore.Authorization;

namespace GameDrop.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Orderdetails(int productId, int quantity)
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

            // Create order details
            var orderDetails = new GameDrop_OrderDetails
            {
                OrderId = order.OrderId,
                ProductId = productId,
                OrderProductName = product.ProductName,
                OrderQuantity = quantity,
                Total = product.ProductPrice * quantity
            };

            _db.OrderDetails.Add(orderDetails);
            await _db.SaveChangesAsync();

            return RedirectToAction("OrderDetails", new { id = order.OrderId });
        }
        //POST: Order details from the shop page
        public IActionResult OrderDetails(int id)
        {
            var orderDetails = _db.OrderDetails
                .Where(od => od.OrderId == id)
                .ToList();

            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }
    }
}