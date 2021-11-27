namespace DotnetMicroserviceArchitecture.Core.Events
{
    public class BasketChangeEvent
    {
        public string UserId { get; set; }
        public string CourseId { get; set; }
        public string UpdateName { get; set; }
    }
}
