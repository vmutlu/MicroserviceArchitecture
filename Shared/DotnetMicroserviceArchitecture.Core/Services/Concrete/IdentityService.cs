using DotnetMicroserviceArchitecture.Core.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace DotnetMicroserviceArchitecture.Core.Services.Concrete
{
    /// <summary>
    /// Get token into user Id info
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        string IIdentityService.GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}
