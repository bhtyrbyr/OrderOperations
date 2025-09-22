using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderOperations.Application.Features.ProductFeatures.Commands;
using OrderOperations.Application.Features.ProductFeatures.Queries;
using OrderOperations.Application.Validator.Product;
using OrderOperations.CustomExceptions.Exceptions.APIExceltions;
using OrderOperations.WebApi.DTOs;
using OrderOperations.WebApi.Languages;

namespace OrderOperations.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IStringLocalizer<Lang> _localizer;
    private readonly IMediator _mediatr;

    public ProductController(IStringLocalizer<Lang> localizer, IMediator mediatr)
    {
        _localizer = localizer;
        _mediatr = mediatr;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediatr.Send(new GetAllProductsQuery());

        if(result == null || !result.Any())
        {
            return NotFound(new ResponseDTO(
                _localizer["errorMsg"].Value,
                _localizer["noProductsFoundMsg"].Value,
                null
            ));
        }

        var response = new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["productsFetchedMsg"].Value,
            result
        );
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediatr.Send(new GetProductByIdQuery(id));

        if (result == null)
        {
            return NotFound(new ResponseDTO(
                _localizer["errorMsg"].Value,
                _localizer["productNotFoundMsg"].Value,
                null
            ));
        }

        var response = new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["productFetchedMsg"].Value,
            result
        );
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductViewModel model)
    {
        var validator = new CreateProductValidator();
        await validator.ValidateAndThrowAsync(model);
        var command = new CreateProductCommand(model);
        var result = await _mediatr.Send(command);

        if (result == Guid.Empty)
        {
            throw new OperationFailException("operationFailedMsg");
        }

        var response = new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["productCreatedMsg"].Value,
            result
        );
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductViewModel model)
    {
        // Body içindeki ID ve route'taki ID eşleşmeli
        if (id != model.Id)
        {
            return BadRequest(new ResponseDTO(
                _localizer["errorMsg"].Value,
                _localizer["productIdMismatchMsg"].Value,
                id
            ));
        }

        var validator = new UpdateProductValidator();
        await validator.ValidateAndThrowAsync(model);

        var command = new UpdateProductCommand(model);

        var result = await _mediatr.Send(command);

        if (!result)
        {
            throw new OperationFailException("operationFailedMsg");
        }

        var response = new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["productUpdatedMsg"].Value,
            id
        );
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var validator = new DeleteProductValidator();
        await validator.ValidateAndThrowAsync(id);

        var result = await _mediatr.Send(new DeleteProductCommand(id));

        if (!result)
        {
            throw new OperationFailException("operationFailedMsg");
        }

        var response = new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["productDeletedMsg"].Value,
            id
        );
        return Ok(response);
    }

    [HttpPost("{productId:guid}/add-stock")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddStock(Guid productId, [FromBody] decimal quantity)
    {
        if (quantity <= 0)
        {
            return BadRequest(new ResponseDTO(
                _localizer["errorMsg"].Value,
                _localizer["quantityMustBeGreaterThanZeroMsg"].Value,
                null
            ));
        }

        var model = new ProductStockUpdateViewModel
        {
            ProductId = productId,
            Quantity = quantity
        };

        var result = await _mediatr.Send(new AddStockToProductCommand(model));

        if (!result)
        {
            throw new OperationFailException("operationFailedMsg");
        }

        var response = new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["productStockAddedMsg"].Value,
            new { ProductId = productId, AddedQuantity = quantity }
        );

        return Ok(response);
    }
}
