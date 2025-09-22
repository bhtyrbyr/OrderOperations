using MediatR;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.CustomExceptions.Exceptions.CommonExceptions;

namespace OrderOperations.Application.Features.ProductFeatures.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<bool>;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingProduct == null)
        {
            throw new NotFoundException("productNotFoundMsg", param1: "modulNameMsg*ProductModule");
        }

        _productRepository.Delete(existingProduct);
        return true;
    }
}
