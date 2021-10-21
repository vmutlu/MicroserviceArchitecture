using DotnetMicroserviceArchitecture.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DotnetMicroserviceArchitecture.Core.CustomControllerBase
{
    public class BaseController : ControllerBase
    {
        public IActionResult Result<T>(Response<T> response) =>
            new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
    }
}
