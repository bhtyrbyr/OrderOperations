using MediatR;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderOperations.Application.Repositories;
using OrderOperations.CustomExceptions.Exceptions.CommonExceptions;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Features.ProductFeatures.Commands;

public record AddStockToProductCommand(ProductStockUpdateViewModel Model) : IRequest<bool>;

public class AddStockToProductHandler : IRequestHandler<AddStockToProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public AddStockToProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(AddStockToProductCommand request, CancellationToken cancellationToken)
    {
        // Ürünü bul
        var product = await _productRepository.GetByIdAsync(request.Model.ProductId, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException("productNotFoundMsg", param1: "modulNameMsg*ProductModule");
        }

        // Stok alanı yoksa yeni bir stok kaydı oluştur
        if (product.Stock == null)
        {
            product.Stock = new Stock
            {
                Id = 0, // EF Core otomatik atayacak
                Amount = request.Model.Quantity
            };
        }
        else
        {
            // Var olan stoğa ekleme yap
            product.Stock.Amount += request.Model.Quantity;
        }

        _productRepository.Update(product);
        return true;
    }
}