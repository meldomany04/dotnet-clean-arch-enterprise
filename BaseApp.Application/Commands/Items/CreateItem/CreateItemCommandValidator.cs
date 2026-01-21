using BaseApp.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaseApp.Application.Commands.Items.CreateItem
{
    public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        public CreateItemCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizer["NameRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["NameLength"]);

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage(localizer["ProductIdMustBePositive"]);
        }
    }
}
