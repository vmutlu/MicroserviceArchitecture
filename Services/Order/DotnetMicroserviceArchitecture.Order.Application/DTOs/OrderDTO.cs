using System;
using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.Order.Application.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime InsertedDate { get; set; }
        public AdressDTO Adress { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}
