using DotnetMicroserviceArchitecture.Core.Commands;
using DotnetMicroserviceArchitecture.Core.CustomControllerBase;
using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.PaymentAPI.Constants;
using DotnetMicroserviceArchitecture.PaymentAPI.DTOs;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider; //Command gönderileceği için sendEndpoint kullanıldı.

        public PaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        //ödeme işlemi için gerekli entegrasyon yapılması gerek. Iyzıco kullanımlarına bak önerilen daha iyi ödeme sistemleri varsa onları entegre et.
        [HttpPost, Route(Route.HTTPGETORPOST_PAYMENTS)]
        public async Task<IActionResult> ReceivePayment(PaymentDTO paymentDTO)
        {
            await SendRabbitMQMessageAsync(paymentDTO).ConfigureAwait(false);

            return Result<NoContent>(Core.Dtos.Response<NoContent>.Success(HttpStatusCode.OK.GetHashCode()));
        }

        private async Task SendRabbitMQMessageAsync(PaymentDTO paymentDTO)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new System.Uri("queue:orderService")).ConfigureAwait(false);

            CreateOrderMessageCommand createOrderMessageCommand = new()
            {
                BuyerId = paymentDTO.Order.BuyerId,
                City = paymentDTO.Order.Adress.City,
                Town = paymentDTO.Order.Adress.Town,
                Line = paymentDTO.Order.Adress.Line,
                Street = paymentDTO.Order.Adress.Street,
                ZipCode = paymentDTO.Order.Adress.ZipCode
            };

            paymentDTO.Order.OrderItems.ForEach(x =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItemCommand()
                {
                    Name = x.Name,
                    OrderId = x.OrderId,
                    Price = x.Price,
                    URL = x.URL
                });
            });

            await _sendEndpointProvider.Send<CreateOrderMessageCommand>(createOrderMessageCommand).ConfigureAwait(false);
        }
    }
}
