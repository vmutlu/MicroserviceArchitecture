using System;

namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class CourseView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get => Description.Length > 80 ? Description.Substring(0, 80) + "..." : Description; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CategoryId { get; set; }
        public CategoryView Category { get; set; }
        public FeaturesView Features { get; set; }
    }
}
