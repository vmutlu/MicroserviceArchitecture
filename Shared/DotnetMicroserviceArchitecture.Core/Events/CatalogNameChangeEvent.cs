namespace DotnetMicroserviceArchitecture.Core.Events
{
    public class CatalogNameChangeEvent
    {
        public string CatalogId { get; set; }
        public string UpdatedName { get; set; }
    }
}
