using DotnetMicroserviceArchitecture.Core.Events;
using DotnetMicroserviceArchitecture.Order.Infrastructure.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.Order.Application.Consumers
{
    public class CatalogNameChangeEventConsumer : IConsumer<CatalogNameChangeEvent>
    {
        private readonly OrderContext _orderContext;

        public CatalogNameChangeEventConsumer(OrderContext orderContext) => (_orderContext) = (orderContext);

        public async Task Consume(ConsumeContext<CatalogNameChangeEvent> context)
        {
            var orderItems = await _orderContext.OrderItems.Where(oi => oi.ProductId == context.Message.CatalogId).ToListAsync().ConfigureAwait(false);
            orderItems.ForEach(u =>
            {
                u.UpdateOrderItem(context.Message.UpdatedName, u.URL, u.Price);
            });

            //catalog microservisinde ki data değişirse order microservisinde ki datayı da güncelle.
            await _orderContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
