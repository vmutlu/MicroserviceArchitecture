using DotnetMicroserviceArchitecture.Core.Commands;
using DotnetMicroserviceArchitecture.Order.Infrastructure.Context;
using MassTransit;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.Order.Application.Consumers
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderContext _orderContext;

        public CreateOrderMessageCommandConsumer(OrderContext orderContext) => (_orderContext) = (orderContext);

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            Domain.Aggregate.Adress newAddress = new(context.Message.City, context.Message.Town, context.Message.Street, context.Message.ZipCode, context.Message.Line);
            Domain.Aggregate.Order order = new(context.Message.BuyerId, newAddress);
            context.Message.OrderItems.ForEach(oi =>
            {
                order.AddOrderItem(oi.OrderId, oi.Name, oi.URL, oi.Price);
            });

            await _orderContext.Orders.AddAsync(order).ConfigureAwait(false);
            await _orderContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
