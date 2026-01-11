using BaseApp.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaseApp.Application.Commands.Products.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizer["NameRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["NameLength"]);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(localizer["PriceMustBePositive"]);
        }
    }
}
