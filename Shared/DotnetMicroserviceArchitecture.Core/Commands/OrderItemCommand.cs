namespace DotnetMicroserviceArchitecture.Core.Commands
{
    public class OrderItemCommand
    {
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public decimal Price { get; set; }
    }
}
