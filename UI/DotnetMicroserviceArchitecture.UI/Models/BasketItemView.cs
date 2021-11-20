namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class BasketItemView
    {
        public string CourseId { get; set; }
        public int Quantity { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
        private decimal? DiscountAppliedPrice { get; set; }
        public decimal GetCurrentPrice { get => DiscountAppliedPrice is not null ? DiscountAppliedPrice.Value : Price; } //Current price info

        public void AppliedDiscount(decimal discountDecimal)
        {
            DiscountAppliedPrice = discountDecimal;
        }
    }
}
