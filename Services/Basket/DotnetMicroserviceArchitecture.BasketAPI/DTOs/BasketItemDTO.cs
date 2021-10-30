namespace DotnetMicroserviceArchitecture.BasketAPI.DTOs
{
    public class BasketItemDTO
    {
        public string CourseId { get; set; }
        public int Quantity { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
    }
}
