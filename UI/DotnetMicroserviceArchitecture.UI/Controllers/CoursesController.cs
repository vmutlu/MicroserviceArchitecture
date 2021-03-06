using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly DotnetMicroserviceArchitecture.Core.Services.Abstract.IIdentityService _identityService;

        public CoursesController(ICatalogService catalogService, DotnetMicroserviceArchitecture.Core.Services.Abstract.IIdentityService identityService)
        {
            _catalogService = catalogService;
            _identityService = identityService;
        }

        public async Task<IActionResult> MyCourses() => View(await _catalogService.GetAllByUserIdAsync(_identityService.GetUserId).ConfigureAwait(false));

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoryAsync().ConfigureAwait(false);

            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateContract courseView)
        {
            var categories = await _catalogService.GetAllCategoryAsync().ConfigureAwait(false);
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            if (!ModelState.IsValid)
                return View();

            courseView.UserId = _identityService.GetUserId;

            await _catalogService.AddAsync(courseView).ConfigureAwait(false);

            return RedirectToAction(nameof(MyCourses));
        }

        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetByIdAsync(id).ConfigureAwait(false);
            var categories = await _catalogService.GetAllCategoryAsync().ConfigureAwait(false);

            if (course == null)
                RedirectToAction(nameof(MyCourses));

            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.Id);

            return View(new CourseUpdateContract()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                Features = course.Features,
                CategoryId = course.CategoryId,
                UserId = course.UserId,
                Picture = course.Picture
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateContract courseView)
        {
            var categories = await _catalogService.GetAllCategoryAsync().ConfigureAwait(false);
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseView.Id);
            if (!ModelState.IsValid)
                return View();

            await _catalogService.UpdateAsync(courseView).ConfigureAwait(false);

            return RedirectToAction(nameof(MyCourses));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _catalogService.DeleteAsync(id).ConfigureAwait(false);

            return RedirectToAction(nameof(MyCourses));
        }
    }
}
