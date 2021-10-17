using DotnetMicroserviceArchitecture.CatalogAPI.Settings.Abstract;

namespace DotnetMicroserviceArchitecture.CatalogAPI.Settings.Concrete
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string CourseCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ConnectionStrings { get; set; }
        public string DatabaseName { get; set; }
    }
}
