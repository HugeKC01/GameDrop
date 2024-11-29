// Controllers/OrderController.cs
using Microsoft.AspNetCore.Mvc;
using GameDrop.Models;
using GameDrop.ViewModels;
using System.Collections.Generic;
using System.Linq;
using GameDrop.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace GameDrop.Controllers
{
    //[Authorize]
    public class OrderController : Controller
    {
        private static List<GameDrop_Order> orders = new List<GameDrop_Order>();
        private static List<GameDrop_OrderDetails> orderDetails = new List<GameDrop_OrderDetails>();
        private readonly DateTimeService _dateTimeService;

        public OrderController(DateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }
        // GET: Order
        public IActionResult Index()
        {
            var viewModelList = orders.Select(order => new OrderViewModel
            {
                Order = order,
                OrderDetails = orderDetails.FirstOrDefault(od => od.OrderId == order.OrderId)
            }).ToList();
            return View(viewModelList);
        }

        // GET: Order/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(OrderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var currentDateTime = await _dateTimeService.GetCurrentDateTimeAsync();
                if (viewModel.Order != null)
                {
                    viewModel.Order.OrderDate = currentDateTime;
                    orders.Add(viewModel.Order);
                }

                if (viewModel.OrderDetails != null)
                {
                    orderDetails.Add(viewModel.OrderDetails);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Order/Edit/5
        public IActionResult Edit(int id)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == id);
            var orderDetail = orderDetails.FirstOrDefault(od => od.OrderId == id);
            if (order == null || orderDetail == null)
            {
                return NotFound();
            }

            var viewModel = new OrderViewModel
            {
                Order = order,
                OrderDetails = orderDetail
            };
            return View(viewModel);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, OrderViewModel viewModel)
        {
            if (id != viewModel.Order.OrderId || id != viewModel.OrderDetails.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var order = orders.FirstOrDefault(o => o.OrderId == id);
                var orderDetail = orderDetails.FirstOrDefault(od => od.OrderId == id);
                if (order != null && orderDetail != null)
                {
                    order.OrderStatus = viewModel.Order.OrderStatus;
                    order.OrderAddress = viewModel.Order.OrderAddress;
                    order.OrderPhone = viewModel.Order.OrderPhone;
                    order.OrderEmail = viewModel.Order.OrderEmail;
                    orderDetail.ProductId = viewModel.OrderDetails.ProductId;
                    orderDetail.ProductName = viewModel.OrderDetails.ProductName;
                    orderDetail.Quantity = viewModel.OrderDetails.Quantity;
                    orderDetail.Total = viewModel.OrderDetails.Total;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Order/Delete/5
        public IActionResult Delete(int id)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == id);
            var orderDetail = orderDetails.FirstOrDefault(od => od.OrderId == id);
            if (order == null || orderDetail == null)
            {
                return NotFound();
            }

            var viewModel = new OrderViewModel
            {
                Order = order,
                OrderDetails = orderDetail
            };
            return View(viewModel);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == id);
            var orderDetail = orderDetails.FirstOrDefault(od => od.OrderId == id);
            if (order != null && orderDetail != null)
            {
                orders.Remove(order);
                orderDetails.Remove(orderDetail);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
