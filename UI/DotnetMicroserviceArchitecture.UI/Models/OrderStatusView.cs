namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class OrderStatusView
    {
        public int OrderId { get; set; }
        public string Error { get; set; }
        public bool IsSuccess { get; set; }
    }
}
