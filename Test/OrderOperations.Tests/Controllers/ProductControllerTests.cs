using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderOperations.Application.Features.ProductFeatures.Commands;
using OrderOperations.Application.Features.ProductFeatures.Queries;
using OrderOperations.WebApi.Controllers;
using OrderOperations.WebApi.DTOs;
using OrderOperations.WebApi.Languages;

namespace OrderOperations.Tests.Controllers;
public class ProductControllerTests
{

    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IStringLocalizer<Lang>> _localizerMock;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _localizerMock = new Mock<IStringLocalizer<Lang>>();

        _localizerMock.Setup(x => x["successMsg"]).Returns(new LocalizedString("successMsg", "Success"));
        _localizerMock.Setup(x => x["errorMsg"]).Returns(new LocalizedString("errorMsg", "Error"));
        _localizerMock.Setup(x => x["productCreatedMsg"]).Returns(new LocalizedString("productCreatedMsg", "Product created successfully"));
        _localizerMock.Setup(x => x["productFetchedMsg"]).Returns(new LocalizedString("productFetchedMsg", "Product fetched successfully"));
        _localizerMock.Setup(x => x["productUpdatedMsg"]).Returns(new LocalizedString("productUpdatedMsg", "Product updated successfully"));
        _localizerMock.Setup(x => x["productDeletedMsg"]).Returns(new LocalizedString("productDeletedMsg", "Product deleted successfully"));

        _controller = new ProductController(_localizerMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WhenProductsExist()
    {
        // Arrange
        var productList = new List<ProductViewModel>
    {
        new ProductViewModel { Id = Guid.NewGuid(), Name = "Product1", Price = 10.0, CategoryName = "Category1" },
        new ProductViewModel { Id = Guid.NewGuid(), Name = "Product2", Price = 20.0, CategoryName = "Category2" }
    };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(productList);

        _localizerMock.Setup(x => x["successMsg"]).Returns(new LocalizedString("successMsg", "Success"));
        _localizerMock.Setup(x => x["productsFetchedMsg"]).Returns(new LocalizedString("productsFetchedMsg", "Products fetched successfully"));
        _localizerMock.Setup(x => x["errorMsg"]).Returns(new LocalizedString("errorMsg", "Error"));
        _localizerMock.Setup(x => x["noProductsFoundMsg"]).Returns(new LocalizedString("noProductsFoundMsg", "No products found"));

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);

        var response = okResult.Value.Should().BeAssignableTo<ResponseDTO>().Subject;
        ((List<ProductViewModel>)response.Data).Count.Should().Be(2);
    }

    [Fact]
    public async Task GetAll_ShouldReturnNotFound_WhenNoProductsExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductViewModel>()); // Boş liste dönüyoruz

        _localizerMock.Setup(x => x["successMsg"]).Returns(new LocalizedString("successMsg", "Success"));
        _localizerMock.Setup(x => x["productsFetchedMsg"]).Returns(new LocalizedString("productsFetchedMsg", "Products fetched successfully"));
        _localizerMock.Setup(x => x["errorMsg"]).Returns(new LocalizedString("errorMsg", "Error"));
        _localizerMock.Setup(x => x["noProductsFoundMsg"]).Returns(new LocalizedString("noProductsFoundMsg", "No products found"));

        // Act
        var result = await _controller.GetAll();

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);

        var response = notFoundResult.Value.Should().BeAssignableTo<ResponseDTO>().Subject;
        response.Data.Should().BeNull();
        response.Message.Should().Be("No products found");
    }

    [Fact]
    public async Task GetById_ShouldReturnSingleProduct()
    {
        // Arrange
        var product = new ProductViewModel
        {
            Id = Guid.NewGuid(),
            Name = "TestProduct",
            Price = 100.0,
            CategoryName = "CategoryTest"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.GetById(product.Id);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<ResponseDTO>().Subject;
        ((ProductViewModel)response.Data).Name.Should().Be("TestProduct");
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedProductId()
    {
        // Arrange
        var createViewModel = new CreateProductViewModel
        {
            Name = "New Product",
            Description = "Product description",
            CategoryId = Guid.NewGuid(),
            Price = 50.0
        };

        var createdProductId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProductId);

        // Act
        var result = await _controller.Create(createViewModel);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<ResponseDTO>().Subject;
        response.Data.Should().Be(createdProductId);
    }

    [Fact]
    public async Task Update_ShouldReturnTrueWhenSuccessful()
    {
        // Arrange
        var updateViewModel = new UpdateProductViewModel
        {
            Id = Guid.NewGuid(),
            Name = "Updated Name",
            Description = "Updated Description",
            CategoryId = Guid.NewGuid(),
            Price = 120.0
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Update(updateViewModel.Id, updateViewModel);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<ResponseDTO>().Subject;
        response.Data.Should().Be(updateViewModel.Id);
    }

    [Fact]
    public async Task Delete_ShouldReturnTrueWhenSuccessful()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(productId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<ResponseDTO>().Subject;
        response.Data.Should().Be(productId);
    }
}
