using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OrderOperations.Application.DTOs.CategoryDTOs;
using OrderOperations.Application.Features.CategoryFeatures.Commands;
using OrderOperations.Application.Features.CategoryFeatures.Queries;
using OrderOperations.Application.Validator.Category;
using OrderOperations.CustomExceptions.Exceptions.APIExceltions;
using OrderOperations.WebApi.DTOs;
using OrderOperations.WebApi.Languages;

namespace OrderOperations.WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IStringLocalizer<Lang> _localizer;
    private readonly IMediator _mediatr;

    public CategoryController(IStringLocalizer<Lang> localizer, IMediator mediatr)
    {
        _localizer = localizer;
        _mediatr = mediatr;
    }

    /// <summary>
    /// Tüm kategorileri getirir.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediatr.Send(new GetAllCategoriesQuery());

        if (result == null || !result.Any())
        {
            return NotFound(new ResponseDTO(
                _localizer["errorMsg"].Value,
                _localizer["noCategoriesFoundMsg"].Value,
                null
            ));
        }

        var response = new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["categoriesFetchedMsg"].Value,
            result
        );
        return Ok(response);
    }

    /// <summary>
    /// Yeni kategori ekler. (Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryViewModel model)
    {
        var validator = new CreateCategoryValidator();
        await validator.ValidateAndThrowAsync(model);

        var result = await _mediatr.Send(new CreateCategoryCommand(model));

        if (result <= 0)
        {
            throw new OperationFailException("operationFailedMsg");
        }

        var response = new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["categoryCreatedMsg"].Value,
            result
        );
        return Ok(response);
    }

    /// <summary>
    /// Kategori siler. (Admin)
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var validator = new DeleteCategoryValidator();
        await validator.ValidateAndThrowAsync(id);

        var result = await _mediatr.Send(new DeleteCategoryCommand(id));

        if (!result)
        {
            throw new OperationFailException("operationFailedMsg");
        }

        var response = new ResponseDTO(
            _localizer["successMsg"].Value,
            _localizer["categoryDeletedMsg"].Value,
            id
        );
        return Ok(response);
    }
}
