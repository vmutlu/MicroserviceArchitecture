using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.PaymentAPI.DTOs
{
    public class OrderDTO
    {
        public OrderDTO()
        {
            OrderItems = new List<OrderItemDTO>();
        }
        public string BuyerId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public AdressDTO Adress { get; set; }
    }
}
