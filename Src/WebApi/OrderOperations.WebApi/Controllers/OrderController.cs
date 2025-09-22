using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OrderOperations.Application.DTOs.OrderDTOs;
using OrderOperations.Application.Features.BasketFeatures.Queries;
using OrderOperations.Application.Features.OrderFeatures.Commands;
using OrderOperations.Application.Features.OrderFeatures.Queries;
using OrderOperations.WebApi.DTOs;
using OrderOperations.WebApi.Languages;
using System.Security.Claims;

namespace OrderOperations.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Sepet işlemleri giriş yapmış kullanıcılar için

    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<Lang> _localizer;

        public OrderController(IMediator mediator, IStringLocalizer<Lang> localizer)
        {
            _mediator = mediator;
            _localizer = localizer;
        }

        private Guid GetCurrentUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllOrder()
        {
            var result = await _mediator.Send(new GetAllOrderQuery(GetCurrentUserId()));
            return Ok(new ResponseDTO(
                _localizer["successMsg"].Value,
                _localizer["orderFetchedMsg"].Value,
                result
            ));
        }

        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(GetCurrentUserId(), orderId));
            return Ok(new ResponseDTO(
                _localizer["successMsg"].Value,
                _localizer["orderFetchedMsg"].Value,
                result
            ));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderFromBasket([FromBody] CreateOrderFromBasketViewModel basket)
        {
            var result = await _mediator.Send(new CreateOrderFromBasketCommand(basket.PersonId, basket.BasketId, basket.IdempotencyKey));

            return Ok(new ResponseDTO(
                _localizer["successMsg"].Value,
                _localizer["orderCreatedFromBasketMsg"].Value,
                result
            ));
        }
    }
}
