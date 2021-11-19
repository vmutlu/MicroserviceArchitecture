using DotnetMicroserviceArchitecture.UI.Exceptions;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Handler
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public TokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //request header added
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _tokenService.GetTokenAsync().ConfigureAwait(false));

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnAuthorizeException();

            return response;
        }
    }
}
