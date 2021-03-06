using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.UI.Models;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Abstract
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SignInModel signInModel);
        Task<Response<bool>> SignUp(SignUpModel signUpModel);
        Task<TokenResponse> GetAccessTokenByRefleshToken();
        Task RemoveRefleshToken();
    }
}
