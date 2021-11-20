﻿using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IStockService _stockService;

        public CatalogService(HttpClient httpClient, IStockService stockService)
        {
            _httpClient = httpClient;
            _stockService = stockService;
        }

        public async Task<bool> AddAsync(CourseCreateContract courseCreateContract)
        {
            var imageRequest = await _stockService.UploadImageAsync(courseCreateContract.PictureFile).ConfigureAwait(false);

            if (imageRequest is not null)
                courseCreateContract.Picture = imageRequest.URL;

            else
                courseCreateContract.Picture = $"{DateTime.Now.Date}_{DateTime.Now.ToShortTimeString()} unsaved picture. response: {imageRequest}";

            var response = await _httpClient.PostAsJsonAsync<CourseCreateContract>("courses/courses", courseCreateContract).ConfigureAwait(false); //send catalog microservice request

            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }

        public async Task<bool> DeleteAsync(string catalogId)
        {
            var response = await _httpClient.DeleteAsync($"courses/courses/{catalogId}").ConfigureAwait(false); //send catalog microservice request

            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }

        /// <summary>
        /// Katalog microservisin GetAll methoduna gönderilen istek
        /// </summary>
        /// <returns></returns>
        public async Task<List<CourseView>> GetAllAsync()
        {
            //http://GatewayURL/Catalog.Path -> appsettings file
            var response = await _httpClient.GetAsync("courses").ConfigureAwait(false); //send catalog microservice request

            if (!response.IsSuccessStatusCode)
                return null;

            var responseData = await response.Content.ReadFromJsonAsync<Response<List<CourseView>>>().ConfigureAwait(false);

            return responseData.Data;
        }

        //courses/getAllByUserId/{id}
        public async Task<List<CourseView>> GetAllByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"courses/getAllByUserId/{userId}").ConfigureAwait(false); //send catalog microservice request

            if (!response.IsSuccessStatusCode)
                return null;

            var responseData = await response.Content.ReadFromJsonAsync<Response<List<CourseView>>>().ConfigureAwait(false);

            return responseData.Data;
        }

        /// <summary>
        ///  Kategori microservisin GetAll methoduna gönderilen istek
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryView>> GetAllCategoryAsync()
        {
            var response = await _httpClient.GetAsync("categories/categories").ConfigureAwait(false); //send category microservice request

            if (!response.IsSuccessStatusCode)
                return null;

            var responseData = await response.Content.ReadFromJsonAsync<Response<List<CategoryView>>>().ConfigureAwait(false);

            return responseData.Data;
        }

        //Courses/{id}
        public async Task<CourseView> GetByIdAsync(string courseId)
        {
            var response = await _httpClient.GetAsync($"courses/courses/{courseId}").ConfigureAwait(false); //send catalog microservice request

            if (!response.IsSuccessStatusCode)
                return null;

            var responseData = await response.Content.ReadFromJsonAsync<Response<CourseView>>().ConfigureAwait(false);

            return responseData.Data;
        }

        public async Task<bool> UpdateAsync(CourseUpdateContract courseUpdateContract)
        {
            var response = await _httpClient.PutAsJsonAsync<CourseUpdateContract>("courses/courses", courseUpdateContract).ConfigureAwait(false); //send catalog microservice request

            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
    }
}
