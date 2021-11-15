using DotnetMicroserviceArchitecture.UI.Models;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Abstract
{
    public interface IUserService
    {
        Task<UserView> GetUserAsync();
    }
}
