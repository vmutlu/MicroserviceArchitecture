using DotnetMicroserviceArchitecture.Order.Core.Entities;

namespace DotnetMicroserviceArchitecture.Order.Domain.Aggregate
{
    public class OrderItem : BaseEntity
    {
        public string OrderId { get; private set; }
        public string Name { get; private set; }
        public string URL { get; private set; }
        public decimal Price { get; private set; }

        public OrderItem(string orderId, string name, string uRL, decimal price)
        {
            OrderId = orderId;
            Name = name;
            URL = uRL;
            Price = price;
        }

        public void UpdateOrderItem(string name, string uRL, decimal price)
        {
            Name = name;
            URL = uRL;
            Price = price;
        }
    }
}
