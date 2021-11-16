using DotnetMicroserviceArchitecture.CatalogAPI.Constants;
using DotnetMicroserviceArchitecture.CatalogAPI.Dtos;
using DotnetMicroserviceArchitecture.CatalogAPI.Services.Abstract;
using DotnetMicroserviceArchitecture.Core.CustomControllerBase;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : BaseController
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService) => (_courseService) = (courseService);

        // api/courses/courses
        [HttpGet, Route(Route.HTTPGET_COURSES)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _courseService.GetAllAsync().ConfigureAwait(false);

            return Result(response);
        }

        // api/courses/{id}
        [HttpGet, Route(Route.HTTPGET_COURSESBYID)]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _courseService.GetByIdAsync(id).ConfigureAwait(false);

            return Result(response);
        }

        // api/courses/getallbyuserid/{id}
        [HttpGet, Route(Route.HTTPGET_COURSESBYUSERID)]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _courseService.GetAllByUserIdAsync(userId).ConfigureAwait(false);

            return Result(response);
        }

        // api/courses
        [HttpPost, Route(Route.HTTPGET_COURSES)]
        public async Task<IActionResult> AddCourse(CourseCreateDTO courseCreateDTO)
        {
            var response = await _courseService.AddAsync(courseCreateDTO).ConfigureAwait(false);

            return Result(response);
        }

        // api/courses
        [HttpPut, Route(Route.HTTPGET_COURSES)]
        public async Task<IActionResult> UpdateCourse(CourseUpdateDTO courseUpdateDTO)
        {
            var response = await _courseService.UpdateAsync(courseUpdateDTO).ConfigureAwait(false);

            return Result(response);
        }

        // api/courses
        [HttpDelete, Route(Route.HTTPGET_COURSESBYID)]
        public async Task<IActionResult> DeleteCourse(string courseId)
        {
            var response = await _courseService.DeleteAsync(courseId).ConfigureAwait(false);

            return Result(response);
        }
    }
}
