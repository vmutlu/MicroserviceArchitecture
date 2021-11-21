using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountView> GetDiscountAsync(string discountCode)
        {
            var response = await _httpClient.GetAsync($"discounts/discounts/GetByCode/{discountCode}").ConfigureAwait(false); //send catalog microservice request

            if (!response.IsSuccessStatusCode)
                return null;

            var responseData = await response.Content.ReadFromJsonAsync<Response<DiscountView>>().ConfigureAwait(false);

            return responseData.Data;
        }
    }
}
