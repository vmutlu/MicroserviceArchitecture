using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using DotnetMicroserviceArchitecture.UI.Settings;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiSettings _apiSettings;
        private readonly ClientSettings _clientSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<ApiSettings> apiSettings, IOptions<ClientSettings> clientSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _apiSettings = apiSettings.Value;
            _clientSettings = clientSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefleshToken()
        {
            // resource: https://identityserver4.readthedocs.io/en/latest/endpoints/discovery.html?highlight=discovery%20end
            // get endpoints
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _apiSettings.IdentityURL,
                Policy = new DiscoveryPolicy() { RequireHttps = false } //default https closed
            }).ConfigureAwait(false);

            if (discoveryDocument.IsError)
                throw discoveryDocument.Exception;

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).ConfigureAwait(false);

            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.UserClientSettings.ClientId,
                ClientSecret = _clientSettings.UserClientSettings.ClientSecret,
                RefreshToken = refreshToken,
                Address = discoveryDocument.TokenEndpoint
            };

            var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest).ConfigureAwait(false);

            if (token.IsError)
                return null;

            var authenticationToken = new List<AuthenticationToken>() {
                new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken } ,
                new AuthenticationToken { Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new AuthenticationToken { Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture) }
            };

            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync().ConfigureAwait(false);

            //tekrar almak yerine alınanı güncelle
            var properties = authenticationResult.Properties;
            properties.StoreTokens(authenticationToken);

            //cookie update
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties).ConfigureAwait(false);

            return token;
        }

        public async Task RemoveRefleshToken()
        {
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _apiSettings.IdentityURL,
                Policy = new DiscoveryPolicy() { RequireHttps = false } //default https closed
            }).ConfigureAwait(false);

            if (discoveryDocument.IsError)
                throw discoveryDocument.Exception;

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).ConfigureAwait(false);

            //revoke refreshToken
            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.UserClientSettings.ClientId,
                ClientSecret = _clientSettings.UserClientSettings.ClientSecret,
                Address = discoveryDocument.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token" // resource: identity documentation
            };

            await _httpClient.RevokeTokenAsync(tokenRevocationRequest).ConfigureAwait(false);
        }


        public async Task<Response<bool>> SignIn(SignInModel signInModel)
        {
            // resource: https://identityserver4.readthedocs.io/en/latest/endpoints/discovery.html?highlight=discovery%20end
            // get endpoints
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _apiSettings.IdentityURL,
                Policy = new DiscoveryPolicy() { RequireHttps = false } //default https closed
            }).ConfigureAwait(false);

            if (discoveryDocument.IsError)
                throw discoveryDocument.Exception;

            var resourceOwnerPassword = new PasswordTokenRequest()
            {
                ClientId = _clientSettings.UserClientSettings.ClientId,
                ClientSecret = _clientSettings.UserClientSettings.ClientSecret,
                UserName = signInModel.Email,
                Password = signInModel.Password,
                Address = discoveryDocument.TokenEndpoint // send request uri (https://demo.identityserver.io/.well-known/openid-configuration)
            };

            var token = await _httpClient.RequestPasswordTokenAsync(resourceOwnerPassword).ConfigureAwait(false);

            if (token.IsError)
            {
                var response = await token.HttpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                var error = JsonSerializer.Deserialize<Error>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return Response<bool>.Fail(error.Errors, HttpStatusCode.NotFound.GetHashCode());
            }

            //UserInfo Endpoint sent request (https://identityserver4.readthedocs.io/en/latest/endpoints/userinfo.html)
            UserInfoRequest userInfoRequest = new()
            {
                Token = token.AccessToken,
                Address = discoveryDocument.UserInfoEndpoint
            };

            var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest).ConfigureAwait(false);

            if (userInfo.IsError)
                throw userInfo.Exception;

            // cookie create
            ClaimsIdentity claimsIdentity = new(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            AuthenticationProperties authenticationProperties = new();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>() {
                new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken } ,
                new AuthenticationToken { Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new AuthenticationToken { Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture) }
            });

            authenticationProperties.IsPersistent = signInModel.IsRemember;

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties).ConfigureAwait(false);

            return Response<bool>.Success(HttpStatusCode.OK.GetHashCode());
        }
    }
}
