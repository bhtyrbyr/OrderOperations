using FluentValidation;
using OrderOperations.Application.DTOs.AuthorizationDTOs;

namespace OrderOperations.Application.Validator.Authorization;

public class RegisterPersonValidator : AbstractValidator<RegisterUserViewModel>
{
    public RegisterPersonValidator()
    {
        RuleFor(cmd => cmd.Email)
            .EmailAddress().WithMessage("emailErrorMsg");
    }
}
