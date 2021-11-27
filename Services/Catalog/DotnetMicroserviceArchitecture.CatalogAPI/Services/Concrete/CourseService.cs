using AutoMapper;
using DotnetMicroserviceArchitecture.CatalogAPI.Dtos;
using DotnetMicroserviceArchitecture.CatalogAPI.Entities;
using DotnetMicroserviceArchitecture.CatalogAPI.Services.Abstract;
using DotnetMicroserviceArchitecture.CatalogAPI.Settings.Abstract;
using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.Core.Events;
using MassTransit;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.CatalogAPI.Services.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;//event gönderimi için publish kullanımı
        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            var client = new MongoClient(databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Core.Dtos.Response<List<CourseDTO>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync().ConfigureAwait(false);

            if (courses.Any())
                foreach (var course in courses)
                    course.Category = await _categoryCollection.Find<Category>(category => category.Id == course.CategoryId).FirstOrDefaultAsync();

            else
                courses = new List<Course>();

            return Core.Dtos.Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), (int)HttpStatusCode.OK);
        }

        public async Task<Core.Dtos.Response<CourseDTO>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(c => c.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

            if (course is null)
                return Core.Dtos.Response<CourseDTO>.Fail("Course Not Found", (int)HttpStatusCode.NotFound);

            course.Category = await _categoryCollection.Find<Category>(category => category.Id == course.CategoryId).FirstOrDefaultAsync();

            return Core.Dtos.Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(course), (int)HttpStatusCode.OK);
        }

        public async Task<Core.Dtos.Response<List<CourseDTO>>> GetAllByUserIdAsync(string userId)
        {
            var userCourses = await _courseCollection.Find<Course>(c => c.UserId == userId).ToListAsync().ConfigureAwait(false);

            if (!userCourses.Any())
                userCourses = new List<Course>();

            else
                foreach (var course in userCourses)
                    course.Category = await _categoryCollection.Find<Category>(category => category.Id == course.CategoryId).FirstOrDefaultAsync();

            return Core.Dtos.Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(userCourses), (int)HttpStatusCode.OK);
        }

        public async Task<Core.Dtos.Response<CourseDTO>> AddAsync(CourseCreateDTO courseCreateDTO)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDTO);
            newCourse.CreatedTime = DateTime.Now;

            await _courseCollection.InsertOneAsync(newCourse).ConfigureAwait(false);

            return Core.Dtos.Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(newCourse), (int)HttpStatusCode.Created);
        }

        public async Task<Core.Dtos.Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDTO)
        {
            var editCourse = _mapper.Map<Course>(courseUpdateDTO);

            var result = await _courseCollection.FindOneAndReplaceAsync<Course>(c => c.Id == courseUpdateDTO.Id, editCourse).ConfigureAwait(false);

            if (result is null)
                return Core.Dtos.Response<NoContent>.Fail("Course Not Found", (int)HttpStatusCode.NotFound);

            //eventual consistent usage.
            //Catalog microservisinde ki data değişirse bu microservisle ilişkili sepet ve ödeme microservisinde ki datalarında değişmesi için event gönderilmektedir.
            await _publishEndpoint.Publish<CatalogNameChangeEvent>(new CatalogNameChangeEvent() { CatalogId = editCourse.Id, UpdatedName = editCourse.Name }).ConfigureAwait(false);
            await _publishEndpoint.Publish<BasketChangeEvent>(new BasketChangeEvent { UserId = editCourse.UserId, CourseId = editCourse.Id, UpdateName = editCourse.Name }).ConfigureAwait(false);

            return Core.Dtos.Response<NoContent>.Success((int)HttpStatusCode.NoContent);
        }

        public async Task<Core.Dtos.Response<NoContent>> DeleteAsync(string courseId)
        {
            var result = await _courseCollection.DeleteOneAsync<Course>(c => c.Id == courseId).ConfigureAwait(false);

            if (result.DeletedCount > decimal.Zero)
                return Core.Dtos.Response<NoContent>.Success((int)HttpStatusCode.NoContent);

            return Core.Dtos.Response<NoContent>.Fail("Course Not Found", (int)HttpStatusCode.NotFound);
        }
    }
}
