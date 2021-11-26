using System;
using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class OrderView
    {
        public OrderView()
        {
            OrderItems = new List<OrderItemView>();
        }
        public int Id { get; set; }
        public DateTime InsertedDate { get; private set; }
        public string BuyerId { get; private set; }
        public List<OrderItemView> OrderItems { get; private set; }
    }
}
