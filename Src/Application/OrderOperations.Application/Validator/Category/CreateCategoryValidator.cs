using FluentValidation;
using OrderOperations.Application.DTOs.CategoryDTOs;

namespace OrderOperations.Application.Validator.Category;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryViewModel>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("categoryNameRequiredMsg")
            .MaximumLength(100).WithMessage("categoryNameMaxLengthMsg");
    }
}
