using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;
using Microsoft.EntityFrameworkCore;
using GameDrop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameDrop.Services;
using System.Security.Claims;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.X509;

namespace GameDrop.Controllers
{
    public class OrderController : Controller
    {
        private readonly GameDropDBContext _db;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly EmailSettings _emailSettings;
        public OrderController(GameDropDBContext db, ShoppingCartService shoppingCartService, IOptions<EmailSettings> emailSettings)
        {
            _db = db;
            _shoppingCartService = shoppingCartService;
            _emailSettings = emailSettings.Value;
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _shoppingCartService.GetCartItemsByUserIdAsync(userId);

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
        public async Task<IActionResult> OrderDetails()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _shoppingCartService.GetCartItemsByUserIdAsync(userId);

            var userAddresses = await _db.GameDrop_UserAddress
                .Where(ua => ua.UserId == userId)
                .ToListAsync();

            ViewBag.UserAddresses = userAddresses;
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
        public async Task<IActionResult> ProceedToPayment(int selectedAddressId)
        {
            var newOrder = new GameDrop_Order
            {
                OrderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                OrderStatus = "Payment Pending",
                PaymentType = "Not Paid",
                UserAddressId = selectedAddressId
            };

            _db.Orders.Add(newOrder);
            await _db.SaveChangesAsync();

            await AddShoppingCartItemsToOrderDetails(newOrder.OrderId);
            return RedirectToAction("Payment", new { id = newOrder.OrderId, orderDate = newOrder.OrderDate, orderStatus = newOrder.OrderStatus });
        }

        public IActionResult Payment(int id, string orderDate, string orderStatus)
        {
            // Implement your payment view logic here
            ViewBag.OrderId = id;
            ViewBag.OrderDate = orderDate;
            ViewBag.OrderStatus = orderStatus;
            ViewBag.TotalAmount = _db.OrderDetails.Where(od => od.OrderId == id).Sum(od => od.Total);

            return View();

        }
        // complete payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompletePayment(int orderId, string paymentMethod)
        {
            // Implement your payment completion logic here
            // For now, just mark the order as completed and redirect to a confirmation page

            var order = _db.Orders.Find(orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Update the order status to Paid
            order.OrderStatus = "Paid";
            order.PaymentType = paymentMethod;

            _db.SaveChanges();

            return RedirectToAction("OrderSummary", new { id = orderId });
        }

        public async Task<IActionResult> OrderSummary(int id)
        {
            var orderDetails = await _db.OrderDetails
                .Where(od => od.OrderId == id)
                .Include(od => od.Product)
                .ToListAsync();

            if (!orderDetails.Any())
            {
                return NotFound();
            }

            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.OrderId = id;
            ViewBag.OrderDate = order.OrderDate;
            return View(orderDetails);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateInvoice(int orderId)
        {
            var orderDetails = await _db.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Product)
                .ToListAsync();

            if (!orderDetails.Any())
            {
                return NotFound();
            }

            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var memoryStream = new MemoryStream();
            var document = new Document();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            writer.CloseStream = false;
            document.Open();

            // Add content to the PDF
            document.Add(new Paragraph("Invoice"));
            document.Add(new Paragraph($"Order ID: {order.OrderId}"));
            document.Add(new Paragraph($"Order Date: {order.OrderDate}"));
            document.Add(new Paragraph($"Order Status: {order.OrderStatus}"));
            document.Add(new Paragraph(" "));

            var table = new PdfPTable(4);
            table.AddCell("Product Name");
            table.AddCell("Quantity");
            table.AddCell("Price");
            table.AddCell("Total");

            foreach (var detail in orderDetails)
            {
                table.AddCell(detail.OrderProductName);
                table.AddCell(detail.OrderQuantity.ToString());
                table.AddCell(detail.Product.ProductPrice.ToString("C"));
                table.AddCell(detail.Total.ToString("C"));
            }

            document.Add(table);
            document.Close();

            memoryStream.Position = 0;
            return File(memoryStream, "application/pdf", $"Invoice_{order.OrderId}.pdf");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailWithAttachment(GameDrop_Order order, MemoryStream pdfStream, int orderId)
        {
            var memoryStream = new MemoryStream();
            await GenerateInvoice(orderId);

            var email = User.FindFirstValue(ClaimTypes.Email); // Get the email from the user's claims
            if (string.IsNullOrEmpty(email))
            {
                throw new InvalidOperationException("User email not found.");
            }

            var subject = "Your Invoice for OrderID {order.OrderId}";
            var body = $"Dear Customer,\n\nPlease find attached the invoice for your order {order.OrderId}.\n\nThank you for your purchase!";

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_emailSettings.FromEmail);
                message.To.Add(email);
                message.Subject = subject;
                message.Body = body;

                // Attach the PDF
                pdfStream.Position = 0;
                var attachment = new Attachment(pdfStream, $"Invoice_{order.OrderId}.pdf", "application/pdf");
                message.Attachments.Add(attachment);

                using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.FromPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    await smtpClient.SendMailAsync(message);
                }
            }
            return RedirectToAction("OrderSummary", new { id = orderId });
        }
    }
}