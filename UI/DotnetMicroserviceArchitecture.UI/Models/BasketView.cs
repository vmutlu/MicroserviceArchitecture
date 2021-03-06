using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class BasketView
    {
        public BasketView()
        {
            basketItems = new List<BasketItemView>();
        }
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        private List<BasketItemView> basketItems { get; set; }
        public decimal TotalPrice { get => basketItems.Sum(x => x.GetCurrentPrice * x.Quantity); }
        public bool HasDiscount { get => !string.IsNullOrWhiteSpace(DiscountCode) && DiscountRate.HasValue; }

        public List<BasketItemView> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    basketItems.ForEach(x =>
                    {
                        var discountPrice =x.Price * ((decimal)DiscountRate.Value / 100);
                        x.AppliedDiscount(Math.Round(x.Price - discountPrice, 2));
                    });
                }

                return basketItems;
            }
            set { basketItems = value; }
        }

        public void CancelDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }

        public void ApplyDiscount(string code, int rate)
        {
            DiscountCode = code;
            DiscountRate = rate;
        }
    }
}
