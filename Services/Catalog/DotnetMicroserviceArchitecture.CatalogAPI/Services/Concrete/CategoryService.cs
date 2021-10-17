using AutoMapper;
using DotnetMicroserviceArchitecture.CatalogAPI.Dtos;
using DotnetMicroserviceArchitecture.CatalogAPI.Entities;
using DotnetMicroserviceArchitecture.CatalogAPI.Services.Abstract;
using DotnetMicroserviceArchitecture.CatalogAPI.Settings.Abstract;
using DotnetMicroserviceArchitecture.Core.Dtos;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.CatalogAPI.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IDatabaseSettings _databaseSettings;
        private readonly IMapper _mapper;
        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDTO>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find<Category>(category => true).ToListAsync().ConfigureAwait(false);

            return Response<List<CategoryDTO>>.Success(_mapper.Map<List<CategoryDTO>>(categories), (int)HttpStatusCode.OK);
        }

        public async Task<Response<CategoryDTO>> AddAsync(CategoryDTO categoryDTO)
        {
            await _categoryCollection.InsertOneAsync(_mapper.Map<Category>(categoryDTO)).ConfigureAwait(false);

            return Response<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(categoryDTO), (int)HttpStatusCode.Created);
        }

        public async Task<Response<CategoryDTO>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find<Category>(c => c.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

            if(category is null)
                return Response<CategoryDTO>.Fail("Category Not Found", (int)HttpStatusCode.NotFound);

            return Response<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category), (int)HttpStatusCode.OK);
        }
    }
}
