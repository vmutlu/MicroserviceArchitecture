using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly HttpClient _httpClient;
        private readonly DotnetMicroserviceArchitecture.Core.Services.Abstract.IIdentityService _identityService;

        public OrderService(HttpClient httpClient, IPaymentService paymentService, IBasketService basketService, DotnetMicroserviceArchitecture.Core.Services.Abstract.IIdentityService identityService)
        {
            _httpClient = httpClient;
            _paymentService = paymentService;
            _basketService = basketService;
            _identityService = identityService;
        }

        public async Task<OrderStatusView> AddOrder(CheckOutInfoRequest checkOutInfoRequest)
        {
            var basket = await _basketService.GetAsync().ConfigureAwait(false);

            PaymentInfoRequest paymentInfoRequest = new()
            {
                CardName = checkOutInfoRequest.CardName,
                CardNumber = checkOutInfoRequest.CardNumber,
                Cvv = checkOutInfoRequest.Cvv,
                Expiration = checkOutInfoRequest.Expiration,
                TotalPrice = basket.TotalPrice
            };

            var response = await _paymentService.ReceivePayment(paymentInfoRequest).ConfigureAwait(false);

            if (!response)
                return new OrderStatusView() { Error = "Payment failed", IsSuccess = false };

            OrderCreateRequest orderCreateRequest = new()
            {
                BuyerId = _identityService.GetUserId,//current user id
                Adress = new AdressView() { City = checkOutInfoRequest.City, Town = checkOutInfoRequest.Town, Line = checkOutInfoRequest.Line, Street = checkOutInfoRequest.Street, ZipCode = checkOutInfoRequest.ZipCode }
            };

            basket.BasketItems.ForEach(basketItem =>
            {
                var order = new OrderItemView() { OrderId = basketItem.CourseId, Price = basketItem.GetCurrentPrice, URL = string.Empty, Name = basketItem.CourseName };
                orderCreateRequest.OrderItems.Add(order);
            });

            var responseRequest = await _httpClient.PostAsJsonAsync<OrderCreateRequest>("orders/orders", orderCreateRequest).ConfigureAwait(false);

            if (!responseRequest.IsSuccessStatusCode)
                return new OrderStatusView() { Error = "Order could not be created", IsSuccess = false };

            var orderCreated = await responseRequest.Content.ReadFromJsonAsync<Response<OrderStatusView>>().ConfigureAwait(false);
            orderCreated.Data.IsSuccess = true;

            if (orderCreated.Data is not null)
                await _basketService.DeleteAsync().ConfigureAwait(false); //ödeme başarılıysa sepeti temizle

            return orderCreated.Data;
        }

        public async Task<List<OrderView>> GetOrderAsync()
        {
            var response = await _httpClient.GetAsync("orders/orders").ConfigureAwait(false); 

            if (!response.IsSuccessStatusCode)
                return null;

            var responseData = await response.Content.ReadFromJsonAsync<Response<List<OrderView>>>().ConfigureAwait(false);

            return responseData.Data;
        }

        public async Task<OrderSuspendView> SuspendOrderAsync(CheckOutInfoRequest checkOutInfoRequest)
        {
            var basket = await _basketService.GetAsync().ConfigureAwait(false);

            OrderCreateRequest orderCreateInput = new()
            {
                BuyerId = _identityService.GetUserId,
                Adress = new AdressView { City = checkOutInfoRequest.City, Town = checkOutInfoRequest.Town, Street = checkOutInfoRequest.Street, Line = checkOutInfoRequest.Line, ZipCode = checkOutInfoRequest.ZipCode },
            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemView { OrderId = x.CourseId, Price = x.GetCurrentPrice, URL = string.Empty, Name = x.CourseName };
                orderCreateInput.OrderItems.Add(orderItem);
            });

            PaymentInfoRequest paymentInfoInput = new()
            {
                CardName = checkOutInfoRequest.CardName,
                CardNumber = checkOutInfoRequest.CardNumber,
                Expiration = checkOutInfoRequest.Expiration,
                Cvv = checkOutInfoRequest.Cvv,
                TotalPrice = basket.TotalPrice,
                Order = orderCreateInput
            };

            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput).ConfigureAwait(false);

            if (!responsePayment)
                return new OrderSuspendView() { Error = "Ödeme alınamadı", IsSuccess = false };

            await AddOrder(checkOutInfoRequest).ConfigureAwait(false);
            await _basketService.DeleteAsync().ConfigureAwait(false);
            return new OrderSuspendView() { IsSuccess = true };
        }
    }
}
