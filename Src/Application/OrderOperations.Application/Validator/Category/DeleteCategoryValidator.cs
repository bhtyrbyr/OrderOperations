using FluentValidation;

namespace OrderOperations.Application.Validator.Category;

public class DeleteCategoryValidator : AbstractValidator<int>
{
    public DeleteCategoryValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0).WithMessage("categoryIdMustBeGreaterThanZeroMsg");
    }
}