using AutoMapper;
using MediatR;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderOperations.Application.Repositories;

namespace OrderOperations.Application.Features.ProductFeatures.Queries;

public record GetAllProductsQuery() : IRequest<List<ProductViewModel>>;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductViewModel>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(ICategoryRepository categoryRepository, IProductRepository productRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductViewModel>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync();
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<List<ProductViewModel>>(products);
    }
}
