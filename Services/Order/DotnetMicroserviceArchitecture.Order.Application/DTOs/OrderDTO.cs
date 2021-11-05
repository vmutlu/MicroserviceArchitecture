using System;
using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.Order.Application.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime InsertedDate { get; private set; }
        public AdressDTO Adress { get; private set; }
        public string BuyerId { get; private set; }
        public List<OrderItemDTO> OrderItemDTOs { get; private set; }
    }
}
