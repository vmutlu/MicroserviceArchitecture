namespace DotnetMicroserviceArchitecture.Order.Application.DTOs
{
    public class OrderItemDTO
    {
        public string OrderId { get; private set; }
        public string Name { get; private set; }
        public string URL { get; private set; }
        public decimal Price { get; private set; }
    }
}
