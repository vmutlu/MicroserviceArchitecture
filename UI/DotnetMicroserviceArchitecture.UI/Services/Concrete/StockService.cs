using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeleteAsync(string imageURL)
        {
            if (imageURL is null)
                return false;

            var response = await _httpClient.DeleteAsync($"photos/{imageURL}").ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<StockView> UploadImageAsync(IFormFile formFile)
        {
            if (formFile is null || formFile.Length <= decimal.Zero)
                return null;

            var newFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(formFile.FileName)}";

            using var memoryStream = new MemoryStream();

            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(memoryStream.ToArray()), "images", newFileName);

            var response = await _httpClient.PostAsync("photos/photos", multipartContent).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseMap = await response.Content.ReadFromJsonAsync<Response<StockView>>().ConfigureAwait(false);

            return responseMap.Data;
        }
    }
}
