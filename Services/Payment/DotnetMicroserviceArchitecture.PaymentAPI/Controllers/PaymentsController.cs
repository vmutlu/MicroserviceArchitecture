using DotnetMicroserviceArchitecture.Core.CustomControllerBase;
using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.PaymentAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotnetMicroserviceArchitecture.PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseController
    {
        //ödeme işlemi için gerekli entegrasyon yapılması gerek. Iyzıco kullanımlarına bak önerilen daha iyi ödeme sistemleri varsa onları entegre et.
        [HttpPost]
        public IActionResult ReceivePayment(PaymentDTO paymentDTO) => Result<NoContent>(Response<NoContent>.Success(HttpStatusCode.OK.GetHashCode()));
    }
}
