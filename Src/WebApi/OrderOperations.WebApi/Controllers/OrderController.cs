using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OrderOperations.Application.DTOs.OrderDTOs;
using OrderOperations.Application.Features.OrderFeatures.Commands;
using OrderOperations.WebApi.DTOs;
using OrderOperations.WebApi.Languages;

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
