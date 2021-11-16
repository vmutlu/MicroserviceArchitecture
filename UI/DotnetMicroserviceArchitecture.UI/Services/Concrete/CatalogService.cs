using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<bool> AddAsync(CourseCreateContract courseCreateContract)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAsync(string catalogId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<CourseView>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<CourseView>> GetAllByUserIdAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<CourseView>> GetAllCategoryAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<CourseView> GetByCatalogIdAsync(string catalogId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAsync(CourseUpdateContract courseUpdateContract)
        {
            throw new System.NotImplementedException();
        }
    }
}
