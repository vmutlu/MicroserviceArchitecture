using System;

namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class CourseView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CategoryId { get; set; }
        public CategoryView CategoryView { get; set; }
        public FeaturesView FeaturesView { get; set; }
    }
}
