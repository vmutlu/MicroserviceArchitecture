using DotnetMicroserviceArchitecture.Order.Core.Entities;

namespace DotnetMicroserviceArchitecture.Order.Domain.Aggregate
{
    public class OrderItem : BaseEntity
    {
        public string ProductId { get; private set; }
        public string Name { get; private set; }
        public string URL { get; private set; }
        public decimal Price { get; private set; }

        public OrderItem()
        {

        }

        public OrderItem(string productIdId, string name, string uRL, decimal price)
        {
            ProductId = productIdId;
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
