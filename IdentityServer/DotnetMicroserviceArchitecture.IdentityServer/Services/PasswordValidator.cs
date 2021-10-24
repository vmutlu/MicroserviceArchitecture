using DotnetMicroserviceArchitecture.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.IdentityServer.Services
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordValidator(UserManager<ApplicationUser> userManager) => (_userManager) = (userManager);

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var exits = await _userManager.FindByEmailAsync(context.UserName).ConfigureAwait(false);

            if (exits is null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string>() { "Email veya şifreniz hatalı." });

                context.Result.CustomResponse = errors;

                return;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(exits, context.Password).ConfigureAwait(false);

            if (passwordCheck is false)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string>() { "Email veya şifreniz hatalı." });

                context.Result.CustomResponse = errors;

                return;
            }

            context.Result = new GrantValidationResult(exits.Id,OidcConstants.AuthenticationMethods.Password);
        }
    }
}
