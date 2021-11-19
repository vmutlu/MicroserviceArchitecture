using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using DotnetMicroserviceArchitecture.UI.Settings;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class TokenService : ITokenService
    {
        private readonly ApiSettings _apiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _httpClient;

        public TokenService(IOptions<ApiSettings> apiSettings, IOptions<ClientSettings> clientSettings, IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient)
        {
            _apiSettings = apiSettings.Value;
            _clientSettings = clientSettings.Value;
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
        {
            //token cache var mı kontrol et
            var currentToken = await _clientAccessTokenCache.GetAsync("ClientToken").ConfigureAwait(false);

            if (currentToken is not null)
                return currentToken.AccessToken;

            // resource: https://identityserver4.readthedocs.io/en/latest/endpoints/discovery.html?highlight=discovery%20end
            // get endpoints
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _apiSettings.IdentityURL,
                Policy = new DiscoveryPolicy() { RequireHttps = false } //default https closed
            }).ConfigureAwait(false);

            if (discoveryDocument.IsError)
                throw discoveryDocument.Exception;

            ClientCredentialsTokenRequest clientCredentialsTokenRequest = new()
            {
                ClientId = _clientSettings.UserClientSettings.ClientId,
                ClientSecret = _clientSettings.UserClientSettings.ClientSecret,
                Address = discoveryDocument.TokenEndpoint
            };

            //token istegi gönderildi
            var request = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest).ConfigureAwait(false);

            if (request.IsError)
                throw request.Exception;

            //alınan tokeni cache at
            await _clientAccessTokenCache.SetAsync("ClientToken", request.AccessToken, request.ExpiresIn).ConfigureAwait(false);

            return request.AccessToken;
        }
    }
}
