using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.Core.Commands
{
    public class CreateOrderMessageCommand
    {
        public string BuyerId { get; set; }
        public List<OrderItemCommand> OrderItems { get; set; } 
        public string City { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }
    }
}
