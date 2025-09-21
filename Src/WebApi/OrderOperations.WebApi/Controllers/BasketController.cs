using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OrderOperations.Application.DTOs.BasketDTOs;
using OrderOperations.Application.Features.BasketFeatures.Commands;
using OrderOperations.Application.Features.BasketFeatures.Queries;
using OrderOperations.CustomExceptions.Exceptions.APIExceltions;
using OrderOperations.WebApi.DTOs;
using OrderOperations.WebApi.Languages;
using System.Security.Claims;

namespace OrderOperations.WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize] // Sepet işlemleri giriş yapmış kullanıcılar için
public class BasketController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<Lang> _localizer;

    public BasketController(IMediator mediator, IStringLocalizer<Lang> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    private Guid GetCurrentUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    private bool IsAdmin() => User.IsInRole("Admin");

    [HttpPost("create")]
    public async Task<IActionResult> CreateBasket()
    {
        var result = await _mediator.Send(new CreateBasketCommand(GetCurrentUserId()));

        return Ok(new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["basketCreatedMsg"].Value,
            result
        ));
    }

    [HttpGet("")]
    public async Task<IActionResult> GetMyBasket()
    {
        var result = await _mediator.Send(new GetMyBasketQuery(GetCurrentUserId()));
        return Ok(new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["basketFetchedMsg"].Value,
            result
        ));
    }

    [HttpGet("{basketId:guid}")]
    public async Task<IActionResult> GetBasket(Guid basketId)
    {
        var result = await _mediator.Send(new GetBasketQuery(basketId, GetCurrentUserId(), IsAdmin()));
        return Ok(new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["basketFetchedMsg"].Value,
            result
        ));
    }

    [HttpDelete("{basketId:guid}")]
    public async Task<IActionResult> DeleteBasket(Guid basketId)
    {
        var result = await _mediator.Send(new DeleteBasketCommand(basketId));
        if (!result)
            throw new OperationFailException("operationFailedMsg");

        return Ok(new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["basketDeletedMsg"].Value,
            basketId
        ));
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductToBasketViewModel model)
    {
        var result = await _mediator.Send(new AddProductToBasketCommand(model));
        if (!result)
            throw new OperationFailException("operationFailedMsg");

        return Ok(new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["productAddedToBasketMsg"].Value,
            model
        ));
    }

    [HttpPost("remove-product")]
    public async Task<IActionResult> RemoveProduct([FromBody] RemoveProductFromBasketViewModel model)
    {
        var result = await _mediator.Send(new RemoveProductFromBasketCommand(model));
        if (!result)
            throw new OperationFailException("operationFailedMsg");

        return Ok(new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["productRemovedFromBasketMsg"].Value,
            model
        ));
    }
}