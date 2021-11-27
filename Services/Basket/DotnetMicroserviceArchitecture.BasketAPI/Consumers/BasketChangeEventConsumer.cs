using DotnetMicroserviceArchitecture.Core.Events;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.Order.Application.Consumers
{
    public class BasketChangeEventConsumer : IConsumer<BasketChangeEvent>
    {
        private readonly BasketAPI.Services.Abstract.IBasketService _basketService;

        public BasketChangeEventConsumer(BasketAPI.Services.Abstract.IBasketService basketService) => (_basketService) = (basketService);

        public async Task Consume(ConsumeContext<BasketChangeEvent> context)
        {
            var items = await _basketService.GetBasket(context.Message.UserId).ConfigureAwait(false);
            items.Data.BasketItems.Where(x => x.CourseId == context.Message.CourseId).ToList().ForEach(b =>
              {
                  b.CourseName = context.Message.UpdateName;
              });

            await _basketService.AddOrUpdate(items.Data).ConfigureAwait(false);
        }
    }
}
