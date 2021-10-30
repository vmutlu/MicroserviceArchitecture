using DotnetMicroserviceArchitecture.BasketAPI.DTOs;
using DotnetMicroserviceArchitecture.BasketAPI.Services.Abstract;
using DotnetMicroserviceArchitecture.Core.Dtos;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.BasketAPI.Services.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService) => (_redisService) = (redisService);

        public async Task<Response<bool>> AddOrUpdate(BasketDTO basketDTO)
        {
            var status = await _redisService.GetDatabase().StringSetAsync(basketDTO.UserId, JsonSerializer.Serialize(basketDTO)).ConfigureAwait(false);

            return status ? Response<bool>.Success(HttpStatusCode.OK.GetHashCode()) : Response<bool>.Fail("Basket could not add", HttpStatusCode.InternalServerError.GetHashCode());
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDatabase().KeyDeleteAsync(userId).ConfigureAwait(false);

            return status ? Response<bool>.Success(HttpStatusCode.NoContent.GetHashCode()) : Response<bool>.Fail("Basket not found", HttpStatusCode.NotFound.GetHashCode());
        }

        public async Task<Response<BasketDTO>> GetBasket(string userId)
        {
            var exitsBasket = await _redisService.GetDatabase().StringGetAsync(userId).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(exitsBasket)) return Response<BasketDTO>.Fail("Basket not found", HttpStatusCode.NotFound.GetHashCode());

            return Response<BasketDTO>.Success(JsonSerializer.Deserialize<BasketDTO>(exitsBasket), HttpStatusCode.OK.GetHashCode());
        }
    }
}
