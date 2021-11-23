using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrdersController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.GetAsync().ConfigureAwait(false);

            ViewBag.basket = basket;
            return View(new CheckOutInfoRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckOutInfoRequest checkOutInfoRequest)
        {
            var orderSuspend = await _orderService.SuspendOrderAsync(checkOutInfoRequest).ConfigureAwait(false);
            if (!orderSuspend.IsSuccess)
            {
                var basket = await _basketService.GetAsync().ConfigureAwait(false);

                ViewBag.basket = basket;
                ViewBag.error = orderSuspend.Error;

                return View();
            }

            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = new Random().Next(1, 1000) });
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }

        public async Task<IActionResult> CheckoutHistory() => View(await _orderService.GetOrderAsync().ConfigureAwait(false));
    }
}
