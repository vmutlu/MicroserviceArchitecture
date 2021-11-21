using DotnetMicroserviceArchitecture.UI.Models;
using FluentValidation;

namespace DotnetMicroserviceArchitecture.UI.Validators
{
    public class CourseUpdateRequestValidator : AbstractValidator<CourseUpdateContract>
    {
        public CourseUpdateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name alanı boş olamaz !");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description alanı boş olamaz !");
            RuleFor(x => x.Features.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Duration alanı boş olamaz !");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price alanı boş olamaz !").ScalePrecision(2, 6).WithMessage("Hatalı price format girdiniz !");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori alanı boş olamaz !");
        }
    }
}
