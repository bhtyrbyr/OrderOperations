using FluentValidation;
using OrderOperations.Application.DTOs.ProductDTOs;

namespace OrderOperations.Application.Validator.Product;

public class CreateProductValidator : AbstractValidator<CreateProductViewModel>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("productNameRequiredMsg")
            .MaximumLength(150).WithMessage("productNameMaxLengthMsg");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("productDescriptionRequiredMsg")
            .MaximumLength(500).WithMessage("productDescriptionMaxLengthMsg");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("productPriceGreaterThanZeroMsg");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("productCategoryRequiredMsg");
    }
}
