using FluentValidation;

namespace OrderOperations.Application.Validator.Product;

public class DeleteProductValidator : AbstractValidator<Guid>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x)
                .NotEmpty().WithMessage("productIdRequiredMsg")
                .Must(x => x != Guid.Empty).WithMessage("productIdCannotBeEmptyMsg");
    }
}
