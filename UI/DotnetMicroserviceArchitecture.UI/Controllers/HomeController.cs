using DotnetMicroserviceArchitecture.UI.Controllers;
using DotnetMicroserviceArchitecture.UI.Exceptions;
using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICatalogService _catalogService;

        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _catalogService.GetAllAsync().ConfigureAwait(false);
            return View(courses);
        }

        public async Task<IActionResult> Detail(string id) => View(await _catalogService.GetByIdAsync(id).ConfigureAwait(false));

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (errorFeature != null && errorFeature.Error is UnAuthorizeException)
                return RedirectToAction(nameof(AuthController.Logout), "Auth");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
