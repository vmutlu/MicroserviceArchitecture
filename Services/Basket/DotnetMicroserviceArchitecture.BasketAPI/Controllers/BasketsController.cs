using DotnetMicroserviceArchitecture.BasketAPI.Constants;
using DotnetMicroserviceArchitecture.BasketAPI.DTOs;
using DotnetMicroserviceArchitecture.BasketAPI.Services.Abstract;
using DotnetMicroserviceArchitecture.Core.CustomControllerBase;
using DotnetMicroserviceArchitecture.Core.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.BasketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : BaseController
    {
        private readonly IBasketService _basketService;
        private readonly IIdentityService _identityService;

        public BasketsController(IIdentityService identityService, IBasketService basketService)
        {
            _identityService = identityService;
            _basketService = basketService;
        }

        [HttpGet,Route(Route.HTTPGETORPOST_BASKETS)]
        public async Task<IActionResult> GetBasket()
        {
            var userId = _identityService.GetUserId;

            return Result(await _basketService.GetBasket(userId).ConfigureAwait(false));
        }

        [HttpPost, Route(Route.HTTPGETORPOST_BASKETS)]
        public async Task<IActionResult> AddOrUpdateBasket(BasketDTO basketDTO)
        {
            basketDTO.UserId = _identityService.GetUserId;
            var response = await _basketService.AddOrUpdate(basketDTO).ConfigureAwait(false);

            return Result(response);
        }

        [HttpDelete, Route(Route.HTTPGETORPOST_BASKETS)]
        public async Task<IActionResult> DeleteBasket()
        {
            var userId = _identityService.GetUserId;

            return Result(await _basketService.Delete(userId));
        }
    }
}
