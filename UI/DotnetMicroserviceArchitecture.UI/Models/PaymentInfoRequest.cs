namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class PaymentInfoRequest
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string Cvv { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
