using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class OrderCreateRequest
    {
        public OrderCreateRequest()
        {
            OrderItems = new List<OrderItemView>();
        }
        public string BuyerId { get; set; }
        public List<OrderItemView> OrderItems { get; set; }
        public AdressView Adress { get; set; }
    }
}
