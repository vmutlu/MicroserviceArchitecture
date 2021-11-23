using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.Order.Application.Commands;
using DotnetMicroserviceArchitecture.Order.Application.DTOs;
using DotnetMicroserviceArchitecture.Order.Domain.Aggregate;
using DotnetMicroserviceArchitecture.Order.Infrastructure.Context;
using MediatR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<InsertedOrderDTO>>
    {
        private readonly OrderContext _orderContext;

        public CreateOrderCommandHandler(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }
        public async Task<Response<InsertedOrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Adress address = new(request?.Adress?.City, request?.Adress?.Town, request?.Adress?.Street, request?.Adress?.ZipCode, request?.Adress?.Line);

            DotnetMicroserviceArchitecture.Order.Domain.Aggregate.Order order = new(request?.BuyerId, address);

            request.OrderItems?.ForEach(x =>
            {
                order.AddOrderItem(x.OrderId, x.Name, x.URL, x.Price);
            });

            await _orderContext.Orders.AddAsync(order).ConfigureAwait(false);

            await _orderContext.SaveChangesAsync().ConfigureAwait(false);

            return Response<InsertedOrderDTO>.Success(new InsertedOrderDTO() { OrderId = order.Id }, HttpStatusCode.OK.GetHashCode());
        }
    }
}
