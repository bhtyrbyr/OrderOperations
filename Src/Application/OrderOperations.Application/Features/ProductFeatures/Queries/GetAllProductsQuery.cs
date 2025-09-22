using AutoMapper;
using MediatR;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderOperations.Application.Interfaces.Repositories;

namespace OrderOperations.Application.Features.ProductFeatures.Queries;

public record GetAllProductsQuery() : IRequest<List<ProductViewModel>>;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductViewModel>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(ICategoryRepository categoryRepository, IStockRepository stockRepository, IProductRepository productRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _stockRepository = stockRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductViewModel>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var stocks = await _stockRepository.GetAllAsync(cancellationToken);  
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<ProductViewModel>>(products.Where(product => product.IsActive).ToList());
    }
}
