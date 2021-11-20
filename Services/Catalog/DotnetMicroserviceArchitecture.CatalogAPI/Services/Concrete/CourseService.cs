using AutoMapper;
using DotnetMicroserviceArchitecture.CatalogAPI.Dtos;
using DotnetMicroserviceArchitecture.CatalogAPI.Entities;
using DotnetMicroserviceArchitecture.CatalogAPI.Services.Abstract;
using DotnetMicroserviceArchitecture.CatalogAPI.Settings.Abstract;
using DotnetMicroserviceArchitecture.Core.Dtos;
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
        private readonly IDatabaseSettings _databaseSettings;
        private readonly IMapper _mapper;
        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDTO>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync().ConfigureAwait(false);

            if (courses.Any())
                courses.Select(async category =>
                {
                    category.Category = await _categoryCollection.Find<Category>(course => course.Id == category.Id).FirstOrDefaultAsync();
                });

            else
                courses = new List<Course>();

            return Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), (int)HttpStatusCode.OK);
        }

        public async Task<Response<CourseDTO>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(c => c.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

            if (course is null)
                return Response<CourseDTO>.Fail("Course Not Found", (int)HttpStatusCode.NotFound);

            course.Category = await _categoryCollection.Find<Category>(c => c.Id == course.Id).FirstOrDefaultAsync();

            return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(course), (int)HttpStatusCode.OK);
        }

        public async Task<Response<List<CourseDTO>>> GetAllByUserIdAsync(string userId)
        {
            var userCourses = await _courseCollection.Find<Course>(c => c.UserId == userId).ToListAsync().ConfigureAwait(false);

            if (!userCourses.Any())
                userCourses = new List<Course>();

            else
                foreach (var course in userCourses)
                    course.Category = await _categoryCollection.Find<Category>(category => category.Id == course.CategoryId).FirstOrDefaultAsync();

            return Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(userCourses), (int)HttpStatusCode.OK);
        }

        public async Task<Response<CourseDTO>> AddAsync(CourseCreateDTO courseCreateDTO)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDTO);
            newCourse.CreatedTime = DateTime.Now;

            await _courseCollection.InsertOneAsync(newCourse).ConfigureAwait(false);

            return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(newCourse), (int)HttpStatusCode.Created);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDTO)
        {
            var editCourse = _mapper.Map<Course>(courseUpdateDTO);

            var result = await _courseCollection.FindOneAndReplaceAsync<Course>(c => c.Id == courseUpdateDTO.Id, editCourse).ConfigureAwait(false);

            if(result is null)
                return Response<NoContent>.Fail("Course Not Found", (int)HttpStatusCode.NotFound);

            return Response<NoContent>.Success((int)HttpStatusCode.NoContent);
        }

        public async Task<Response<NoContent>> DeleteAsync(string courseId)
        {
            var result = await _courseCollection.DeleteOneAsync<Course>(c => c.Id == courseId).ConfigureAwait(false);

            if (result.DeletedCount > decimal.Zero)
                return Response<NoContent>.Success((int)HttpStatusCode.NoContent);

            return Response<NoContent>.Fail("Course Not Found", (int)HttpStatusCode.NotFound);
        }
    }
}
