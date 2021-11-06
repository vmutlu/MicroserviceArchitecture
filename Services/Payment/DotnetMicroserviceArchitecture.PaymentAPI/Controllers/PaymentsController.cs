using DotnetMicroserviceArchitecture.Core.CustomControllerBase;
using DotnetMicroserviceArchitecture.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotnetMicroserviceArchitecture.PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseController
    {
        [HttpPost]
        public IActionResult ReceivePayment() => Result<NoContent>(Response<NoContent>.Success(HttpStatusCode.OK.GetHashCode()));
    }
}
