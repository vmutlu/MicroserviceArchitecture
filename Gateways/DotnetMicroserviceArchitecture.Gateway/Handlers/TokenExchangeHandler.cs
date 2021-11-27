using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.Gateway.Handlers
{
    public class TokenExchangeHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _accessToken;

        public TokenExchangeHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private async Task<string> GetTokenExchangeAsync(string token)
        {
            if (!string.IsNullOrWhiteSpace(_accessToken))
                return _accessToken;

            // resource: https://identityserver4.readthedocs.io/en/latest/endpoints/discovery.html?highlight=discovery%20end
            // get endpoints
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _configuration["IdentityServer"],
                Policy = new DiscoveryPolicy() { RequireHttps = false } //default https closed
            }).ConfigureAwait(false);

            if (discoveryDocument.IsError)
                throw discoveryDocument.Exception;

            TokenExchangeTokenRequest tokenExchangeTokenRequest = new()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                GrantType = _configuration["GrantType"],
                SubjectToken = token,
                SubjectTokenType = _configuration["SubjectTokenType"],
                Scope = _configuration["Scope"]
            };

            var tokenResponse = await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest).ConfigureAwait(false);

            if (tokenResponse.IsError)
                throw tokenResponse.Exception;

            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }

        //araya gir eski tokeni gönder yeni tokeni al aldıgın tokenle discount ve payment microservislerine istek yapabilir durumda ol
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentToken = request.Headers.Authorization.Parameter;

            var newToken = await GetTokenExchangeAsync(currentToken).ConfigureAwait(false);

            request.SetBearerToken(newToken);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
