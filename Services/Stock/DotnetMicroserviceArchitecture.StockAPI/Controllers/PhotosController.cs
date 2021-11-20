using DotnetMicroserviceArchitecture.Core.CustomControllerBase;
using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.StockAPI.Constants;
using DotnetMicroserviceArchitecture.StockAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost,Route(Route.HTTPGETORPOST_PHOTOS)]
        public async Task<IActionResult> AddImage(IFormFile image, CancellationToken cancellationToken)
        {
            if (image != null && image.Length > decimal.Zero)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await image.CopyToAsync(stream, cancellationToken);

                var returnPath = image.FileName;

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
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageUrl);
            if (!System.IO.File.Exists(path))
                return Result(Response<NoContent>.Fail("Image not found", HttpStatusCode.NotFound.GetHashCode()));

            System.IO.File.Delete(path);
            return Result(Response<NoContent>.Success(HttpStatusCode.NoContent.GetHashCode()));
        }
    }
}
