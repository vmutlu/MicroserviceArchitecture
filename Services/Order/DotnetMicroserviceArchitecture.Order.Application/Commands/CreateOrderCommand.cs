using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.Order.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.Order.Application.Commands
{
    public class CreateOrderCommand : IRequest<Response<InsertedOrderDTO>>
    {
        public string BuyerId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public AdressDTO AdressDTO { get; set; }
    }
}
