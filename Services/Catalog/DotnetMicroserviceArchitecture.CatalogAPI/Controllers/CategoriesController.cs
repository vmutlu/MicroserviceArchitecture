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
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService) => (_categoryService) = (categoryService);

        // api/categories
        [HttpGet, Route(Route.HTTPGET_CATEGORIES)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync().ConfigureAwait(false);
            return Result(result);
        }


        // api/categories/{id}
        [HttpGet, Route(Route.HTTPGET_CATEGORIESBYID)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _categoryService.GetByIdAsync(id).ConfigureAwait(false);
            return Result(result);
        }

        // api/categories
        [HttpPost, Route(Route.HTTPGET_CATEGORIES)]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO)
        {
            var result = await _categoryService.AddAsync(categoryDTO).ConfigureAwait(false);
            return Result(result);
        }
    }
}
