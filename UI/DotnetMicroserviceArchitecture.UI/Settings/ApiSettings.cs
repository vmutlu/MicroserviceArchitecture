namespace DotnetMicroserviceArchitecture.UI.Settings
{
    public class ApiSettings
    {
        public string IdentityURL { get; set; }
        public string GatewayURL { get; set; }
        public string ImageURL { get; set; }
        public ServiceAPI Catalog { get; set; }
        public ServiceAPI Stock { get; set; }
        public ServiceAPI Basket { get; set; }
        public ServiceAPI Discount { get; set; }
        public ServiceAPI Payment { get; set; }
        public ServiceAPI Order { get; set; }
    }
}
