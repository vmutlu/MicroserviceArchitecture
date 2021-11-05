using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.Order.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.Order.Application.Querys
{
    public class GetOrderByUserIdQuery : IRequest<Response<List<OrderDTO>>>
    {
        public string UserId { get; set; }
    }
}
