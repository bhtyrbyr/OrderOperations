using MediatR;
using OrderOperations.Application.DTOs.AuthorizationDTOs;
using OrderOperations.Application.TokenOperations;
using OrderOperations.CustomExceptions.Exceptions.AuthExceptions;

namespace OrderOperations.Application.Features.Authorization.Commands;

public record LoginUserCommand(LoginUserViewModel Model) : IRequest<LoginResponseViewModel>;
public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponseViewModel>
{
    private readonly IAuthService _authService;

    public LoginUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginResponseViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var token = await _authService.Login(request.Model.UserName, request.Model.Password);
        if (token.Item2.token is "errorLogin")
            throw new LoginFailedException("loginErrorMsg", param1: "modulNameMsg*AuthorizationModule");
        LoginResponseViewModel response = new LoginResponseViewModel();
        response.UserId = token.Id;
        response.Token = token.Item2.token;
        response.TokenStartDate = token.Item2.StartDate;
        response.TokenEndDate = token.Item2.EndDate;
        return response;
    }
}
