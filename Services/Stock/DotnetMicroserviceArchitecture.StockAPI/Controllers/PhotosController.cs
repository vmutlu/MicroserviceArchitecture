using DotnetMicroserviceArchitecture.Core.CustomControllerBase;
using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.StockAPI.Constants;
using DotnetMicroserviceArchitecture.StockAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.StockAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwroot;
        private readonly string _imgFolder = "images";

        public PhotosController(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = _env.WebRootPath;
        }

        [HttpPost,Route(Route.HTTPGETORPOST_PHOTOS)]
        public async Task<IActionResult> AddImage(IFormFile images, CancellationToken cancellationToken)
        {
            if (images != null && images.Length > decimal.Zero)
            {
                if (!Directory.Exists($"{_wwwroot}\\{_imgFolder}"))
                    Directory.CreateDirectory($"{_wwwroot}\\{_imgFolder}");

                var path = Path.Combine($"{_wwwroot}\\{_imgFolder}\\", images.FileName);

               // Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", images.FileName);

                /// using var stream = new FileStream(path, FileMode.Create);
                await using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await images.CopyToAsync(stream);
                }

                //await images.CopyToAsync(stream, cancellationToken);

                var returnPath = images.FileName;

                var imageDTO = new ImageDTO()
                {
                    URL = returnPath
                };

                return Result(Response<ImageDTO>.Success(imageDTO, HttpStatusCode.OK.GetHashCode()));
            }

            return Result(Response<ImageDTO>.Fail("Image is empty", HttpStatusCode.NotFound.GetHashCode()));
        }

        [HttpDelete, Route(Route.HTTPDELETE_PHOTOS)]
        public IActionResult DeleteImage(string imageUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", imageUrl);
            if (!System.IO.File.Exists(path))
                return Result(Response<NoContent>.Fail("Image not found", HttpStatusCode.NotFound.GetHashCode()));

            System.IO.File.Delete(path);
            return Result(Response<NoContent>.Success(HttpStatusCode.NoContent.GetHashCode()));
        }
    }
}
