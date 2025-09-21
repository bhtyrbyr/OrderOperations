using AutoMapper;
using MediatR;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderOperations.Application.Repositories;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Features.ProductFeatures.Commands;


public record CreateProductCommand(CreateProductViewModel Model) : IRequest<Guid>;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IMapper _mapper;

    public CreateProductHandler(ICategoryRepository categoryRepository, IProductRepository productRepository, IStockRepository stockRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _stockRepository = stockRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request.Model);

        var category = await _categoryRepository.GetByIdAsync(request.Model.CategoryId);

        product.Category = category is null ? null : category;
        product.Stock = new();

        await _productRepository.CreateAsync(product);
        return product.Id;
    }
}
