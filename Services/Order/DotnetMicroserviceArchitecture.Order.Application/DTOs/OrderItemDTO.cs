namespace DotnetMicroserviceArchitecture.Order.Application.DTOs
{
    public class OrderItemDTO
    {
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public decimal Price { get; set; }
    }
}
