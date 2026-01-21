using FluentValidation;

namespace BaseApp.Application.Commands.Products.UpdateProductList
{
    public class UpdateProductListCommandValidator : AbstractValidator<UpdateProductListCommand>
    {
        public UpdateProductListCommandValidator()
        {
            //RuleFor(x => x.Name)
            //    .NotEmpty()
            //    .MaximumLength(100);

            //RuleFor(x => x.Price)
            //    .GreaterThan(0);
        }
    }
}
