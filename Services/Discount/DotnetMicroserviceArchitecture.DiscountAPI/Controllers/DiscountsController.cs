using DotnetMicroserviceArchitecture.Core.CustomControllerBase;
using DotnetMicroserviceArchitecture.Core.Services.Abstract;
using DotnetMicroserviceArchitecture.DiscountAPI.Entities;
using DotnetMicroserviceArchitecture.DiscountAPI.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.DiscountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IDiscountService _discountService;

        public DiscountsController(IIdentityService identityService, IDiscountService discountService)
        {
            _identityService = identityService;
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Result(await _discountService.GetAll().ConfigureAwait(false));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await _discountService.GetById(id).ConfigureAwait(false);

            return Result(discount);
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = _identityService.GetUserId;

            var discount = await _discountService.GetByCodeAndUserId(code, userId).ConfigureAwait(false);

            return Result(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Discount discount)
        {
            return Result(await _discountService.Add(discount).ConfigureAwait(false));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Discount discount)
        {
            return Result(await _discountService.Update(discount).ConfigureAwait(false));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Result(await _discountService.Delete(id).ConfigureAwait(false));
        }
    }
}
