using DotnetMicroserviceArchitecture.CatalogAPI.Dtos;
using DotnetMicroserviceArchitecture.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.CatalogAPI.Services.Abstract
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDTO>>> GetAllAsync();
        Task<Response<CategoryDTO>> AddAsync(CategoryDTO categoryDTO);
        Task<Response<CategoryDTO>> GetByIdAsync(string id);
    }
}
