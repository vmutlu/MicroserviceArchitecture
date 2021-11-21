using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Controllers
{
    [Authorize]
    public class BasketsController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public BasketsController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index() => View(await _basketService.GetAsync().ConfigureAwait(false));

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetByIdAsync(courseId).ConfigureAwait(false);

            var basketItem = new BasketItemView() { CourseId = course.Id, CourseName = course.Name, Price = course.Price, Quantity = (int)decimal.One };

            await _basketService.AddBasketItemAsync(basketItem).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteBasketItem(string courseId)
        {
            await _basketService.DeleteBasketItemAsync(courseId).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }
    }
}
