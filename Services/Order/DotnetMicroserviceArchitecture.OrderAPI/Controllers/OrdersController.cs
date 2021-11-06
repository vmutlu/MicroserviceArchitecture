using DotnetMicroserviceArchitecture.Core.CustomControllerBase;
using DotnetMicroserviceArchitecture.Core.Services.Abstract;
using DotnetMicroserviceArchitecture.Order.Application.Commands;
using DotnetMicroserviceArchitecture.Order.Application.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public OrdersController(IMediator mediator, IIdentityService identityService)
        {
            _mediator = mediator;
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var response = await _mediator.Send(new GetOrderByUserIdQuery { UserId = _identityService.GetUserId }).ConfigureAwait(false);

            return Result(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrdersAsync(CreateOrderCommand createOrderCommand)
        {
            var response = await _mediator.Send(createOrderCommand).ConfigureAwait(false);

            return Result(response);
        }
    }
}
