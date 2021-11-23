namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class CheckOutInfoRequest
    {
        public string City { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string Cvv { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
