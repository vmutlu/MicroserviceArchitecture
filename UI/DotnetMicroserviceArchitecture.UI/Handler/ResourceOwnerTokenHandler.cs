using DotnetMicroserviceArchitecture.UI.Exceptions;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Handler
{
    public class ResourceOwnerTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResourceOwnerTokenHandler> _logger;

        public ResourceOwnerTokenHandler(ILogger<ResourceOwnerTokenHandler> logger, IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }

        //middleware gibi her istek sonucu araya gir çalış
        //override method
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).ConfigureAwait(false);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // access token ömrü dolduysa
            {
                var newAcccessToken = await _identityService.GetAccessTokenByRefleshToken().ConfigureAwait(false);

                if (newAcccessToken is not null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAcccessToken.AccessToken);

                    response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false); // re send request
                }
            }

            //response da hala Unauthorized hatası varsa logine gidip tekrar giriş yapsın
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnAuthorizeException();

            return response;
        }
    }
}
