using DotnetMicroserviceArchitecture.UI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Abstract
{
    public interface ICatalogService
    {
        Task<List<CourseView>> GetAllAsync();
        Task<List<CourseView>> GetAllCategoryAsync();
        Task<List<CourseView>> GetAllByUserIdAsync(string userId);
        Task<bool> DeleteAsync(string catalogId);
        Task<bool> AddAsync(CourseCreateContract courseCreateContract);
        Task<bool> UpdateAsync(CourseUpdateContract courseUpdateContract);
        Task<CourseView> GetByCatalogIdAsync(string catalogId);
    }
}
