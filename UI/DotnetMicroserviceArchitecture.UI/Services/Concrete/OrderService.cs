using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System.Net.Http;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class OrderService: IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
