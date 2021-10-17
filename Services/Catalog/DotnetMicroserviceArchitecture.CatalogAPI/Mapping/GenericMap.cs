using AutoMapper;
using DotnetMicroserviceArchitecture.CatalogAPI.Dtos;
using DotnetMicroserviceArchitecture.CatalogAPI.Entities;

namespace DotnetMicroserviceArchitecture.CatalogAPI.Mapping
{
    public class GenericMap : Profile
    {
        public GenericMap()
        {
            #region Course Mappings

            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<Course, CourseCreateDTO>().ReverseMap();
            CreateMap<Course, CourseUpdateDTO>().ReverseMap();

            #endregion

            #region Category Mappings

            CreateMap<Category, CategoryDTO>().ReverseMap();

            #endregion

            #region Feature Mappings

            CreateMap<Feature, FeatureDTO>().ReverseMap();

            #endregion
        }
    }
}
