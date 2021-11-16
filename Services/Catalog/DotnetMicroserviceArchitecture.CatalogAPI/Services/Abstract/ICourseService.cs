using DotnetMicroserviceArchitecture.CatalogAPI.Dtos;
using DotnetMicroserviceArchitecture.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.CatalogAPI.Services.Abstract
{
    public interface ICourseService
    {
        Task<Response<List<CourseDTO>>> GetAllAsync();
        Task<Response<CourseDTO>> GetByIdAsync(string id);
        Task<Response<List<CourseDTO>>> GetAllByUserIdAsync(string userId);
        Task<Response<CourseDTO>> AddAsync(CourseCreateDTO courseCreateDTO);
        Task<Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDTO);
        Task<Response<NoContent>> DeleteAsync(string courseId);
    }
}
