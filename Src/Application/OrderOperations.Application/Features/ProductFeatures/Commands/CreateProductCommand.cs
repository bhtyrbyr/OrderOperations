using AutoMapper;
using MediatR;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderOperations.Application.Repositories;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Features.ProductFeatures.Commands;


public record CreateProductCommand(CreateProductViewModel Model) : IRequest<Guid>;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request.Model);

        await _productRepository.CreateAsync(product);
        return product.Id;
    }
}
