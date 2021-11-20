using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task AddBasketItemAsync(BasketItemView basketItemView)
        {
            var exitsBasket = await GetAsync().ConfigureAwait(false);

            if (exitsBasket is not null)
                if (!exitsBasket.BasketItems.Any(x => x.CourseId == basketItemView.CourseId))
                    exitsBasket.BasketItems.Add(basketItemView);

                else
                {
                    exitsBasket = new BasketView();
                    exitsBasket.BasketItems.Add(basketItemView);
                }

            await AddOrUpdateAsync(exitsBasket).ConfigureAwait(false);
        }

        public async Task<bool> AddOrUpdateAsync(BasketView basketView)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketView>("baskets/baskets", basketView).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ApplyDiscountAsync(string discountCode)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> CancelApplyDiscountAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteAsync()
        {
            var response = await _httpClient.DeleteAsync("baskets/baskets").ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBasketItemAsync(string courseId)
        {
            var response = await GetAsync().ConfigureAwait(false);

            if (response is null)
                return false;

            var deletedItem = response.BasketItems.FirstOrDefault(x => x.CourseId == courseId);

            if (deletedItem is null)
                return false;

            var deleteBasket = response.BasketItems.Remove(deletedItem);

            if (deleteBasket is not true)
                return false;

            if (!response.BasketItems.Any())
                response.DiscountCode = null;

            return await AddOrUpdateAsync(response).ConfigureAwait(false);
        }

        public async Task<BasketView> GetAsync()
        {
            var response = await _httpClient.GetAsync("baskets/baskets").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return null;

            var baskets = await response.Content.ReadFromJsonAsync<Response<BasketView>>().ConfigureAwait(false);

            return baskets.Data;
        }
    }
}
