using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class BasketView
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<BasketItemView> BasketItems { get; set; }
        public decimal TotalPrice { get => BasketItems.Sum(x => x.Price * x.Quantity); }
    }
}
