using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Task AddBasketItemAsync(BasketItemView basketItemView)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddOrUpdateAsync(BasketView basketView)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ApplyDiscountAsync(string discountCode)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CancelApplyDiscountAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteBasketItemAsync(string courseId)
        {
            throw new System.NotImplementedException();
        }

        public Task<BasketView> GetAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
