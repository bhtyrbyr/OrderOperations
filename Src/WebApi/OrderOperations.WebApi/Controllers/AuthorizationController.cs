using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OrderOperations.Application.DTOs.AuthorizationDTOs;
using OrderOperations.Application.Features.Authorization.Commands;
using OrderOperations.Application.Validator.Authorization;
using OrderOperations.CustomExceptions.Exceptions.APIExceltions;
using OrderOperations.WebApi.DTOs;
using OrderOperations.WebApi.Languages;

namespace OrderOperations.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationController : ControllerBase
{
    private readonly IStringLocalizer<Lang> _localizer;
    private readonly IMediator _mediatr;

    public AuthorizationController(IStringLocalizer<Lang> localizer, IMediator mediatr)
    {
        _localizer = localizer;
        _mediatr = mediatr;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Sigup([FromBody] RegisterUserViewModel model)
    {
        var validator = new RegisterPersonValidator();

        await validator.ValidateAndThrowAsync(model);

        var result = await _mediatr.Send(new RegisterUserCommand(model));

        if (string.IsNullOrEmpty(result))
        {
            throw new OperationFailException("operationFailedMsg");
        }

        var response = new ResponseDTO(_localizer["successMsg"].Value, _localizer["registrationSuccessMsg"].Value, result);
        return Ok(response);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Sigin([FromBody] LoginUserViewModel model)
    {

        var result = await _mediatr.Send(new LoginUserCommand(model));

        if (result is null)
        {
            throw new OperationFailException("operationFailedMsg");
        }

        var response = new ResponseDTO(_localizer["successMsg"].Value, _localizer["loginSuccessMsg"].Value, result);
        return Ok(response);
    }
}
