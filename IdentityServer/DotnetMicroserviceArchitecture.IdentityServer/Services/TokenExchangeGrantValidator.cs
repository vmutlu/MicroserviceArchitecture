using IdentityServer4.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.IdentityServer.Services
{
    public class TokenExchangeGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "urn:ietf:params:oauth:grant-type:token-exchange"; //best practive: urn:ietf:params:oauth:grant-type:

        private readonly ITokenValidator _tokenValidator;

        public TokenExchangeGrantValidator(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var requestRaw = context.Request.Raw.ToString();

            var token = context.Request.Raw.Get("subject_token");

            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest, "token missing");
                return;
            }

            var tokenValidate = await _tokenValidator.ValidateAccessTokenAsync(token).ConfigureAwait(false);

            if (tokenValidate.IsError)
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant, "token invalid");
                return;
            }

            var subject = tokenValidate.Claims.FirstOrDefault(c => c.Type == "sub");

            if (subject is null)
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant, "token must contain sub value");
                return;
            }

            //new access token
            context.Result = new GrantValidationResult(subject.Value, "access_token", tokenValidate.Claims);
            return;
        }
    }
}
