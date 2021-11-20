using Microsoft.AspNetCore.Http;

namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class CourseCreateContract
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }
        public string CategoryId { get; set; }
        public FeaturesView Features { get; set; }
        public IFormFile PictureFile { get; set; }
    }
}
