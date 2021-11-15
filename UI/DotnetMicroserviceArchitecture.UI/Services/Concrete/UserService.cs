using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // */ GetFromJsonAsync */ Net 5.0 Automatic deserialize 
        public async Task<UserView> GetUserAsync() => await _httpClient.GetFromJsonAsync<UserView>("/api/users/getUserInfo").ConfigureAwait(false);
    }
}
