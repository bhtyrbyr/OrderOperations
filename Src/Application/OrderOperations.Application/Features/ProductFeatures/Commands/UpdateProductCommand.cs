using AutoMapper;
using MediatR;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.CustomExceptions.Exceptions.CommonExceptions;

namespace OrderOperations.Application.Features.ProductFeatures.Commands;

public record UpdateProductCommand(UpdateProductViewModel Model) : IRequest<bool>;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetByIdAsync(request.Model.Id, cancellationToken);
        if (existingProduct == null)
        {
            throw new NotFoundException("productNotFoundMsg", param1: "modulNameMsg*ProductModule");
        }

        // Mevcut entity'yi güncelle
        _mapper.Map(request.Model, existingProduct);

        _productRepository.Update(existingProduct);
        return true;
    }
}
