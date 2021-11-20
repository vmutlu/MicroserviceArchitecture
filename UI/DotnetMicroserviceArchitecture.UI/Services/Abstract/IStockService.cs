using DotnetMicroserviceArchitecture.UI.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Abstract
{
    public interface IStockService
    {
        Task<StockView> UploadImageAsync(IFormFile formFile);
        Task<bool> DeleteAsync(string imageURL);
    }
}
