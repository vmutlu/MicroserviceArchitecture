using DotnetMicroserviceArchitecture.UI.Models;
using FluentValidation;

namespace DotnetMicroserviceArchitecture.UI.Validators
{
    public class DiscountApplyViewValidator : AbstractValidator<DiscountApplyView>
    {
        public DiscountApplyViewValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code alanı boş olamaz !");
        }
    }
}
