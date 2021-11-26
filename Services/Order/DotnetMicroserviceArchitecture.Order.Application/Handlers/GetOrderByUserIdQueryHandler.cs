using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.Order.Application.DTOs;
using DotnetMicroserviceArchitecture.Order.Application.Mapping;
using DotnetMicroserviceArchitecture.Order.Application.Querys;
using DotnetMicroserviceArchitecture.Order.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.Order.Application.Handlers
{
    public class GetOrderByUserIdQueryHandler : IRequestHandler<GetOrderByUserIdQuery, Response<List<OrderDTO>>>
    {
        private readonly OrderContext _orderContext;

        public GetOrderByUserIdQueryHandler(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task<Response<List<OrderDTO>>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderContext.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == request.UserId).ToListAsync().ConfigureAwait(false);

            if (!orders.Any())
                return Response<List<OrderDTO>>.Success(new List<OrderDTO>(), HttpStatusCode.OK.GetHashCode());

            var mappedOrderDTO = ObjectMapper.Mapper.Map<List<OrderDTO>>(orders);

            return Response<List<OrderDTO>>.Success(mappedOrderDTO, HttpStatusCode.OK.GetHashCode());
        }
    }
}
