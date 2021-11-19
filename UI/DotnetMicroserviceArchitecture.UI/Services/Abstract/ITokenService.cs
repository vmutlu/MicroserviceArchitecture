using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Abstract
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
    }
}
