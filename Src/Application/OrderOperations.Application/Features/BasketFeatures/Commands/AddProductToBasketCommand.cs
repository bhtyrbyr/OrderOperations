using MediatR;
using OrderOperations.Application.DTOs.BasketDTOs;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.CustomExceptions.Exceptions.BasketExceptions;
using OrderOperations.CustomExceptions.Exceptions.CommonExceptions;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Features.BasketFeatures.Commands;

public record AddProductToBasketCommand(AddProductToBasketViewModel Model) : IRequest<bool>;

public class AddProductToBasketHandler : IRequestHandler<AddProductToBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IBasketItemRepository _basketItemRepository;
    private readonly IStockRepository _stockRepository; 
    private readonly IProductRepository _productRepository;

    public AddProductToBasketHandler(IBasketRepository basketRepository, IBasketItemRepository basketItemRepository, IStockRepository stockRepository, IProductRepository productRepository)
    {
        _basketRepository = basketRepository;
        _basketItemRepository = basketItemRepository;
        _stockRepository = stockRepository;
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(AddProductToBasketCommand request, CancellationToken cancellationToken)
    {
        var basketItems = await _basketItemRepository.GetAllAsync(cancellationToken);
        var basket = await _basketRepository.GetByIdAsync(request.Model.BasketId, cancellationToken);
        if (basket == null)
        {
            throw new NotFoundException("basketNotFoundMsg", param1: "modulNameMsg*BasketModule");
        }

        var stocks = await _stockRepository.GetAllAsync(cancellationToken);
        var product = await _productRepository.GetByIdAsync(request.Model.ProductId, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException("productNotFoundMsg", param1: "modulNameMsg*ProductModule");
        }

        // Stok kontrolü
        if (product.Stock == null || product.Stock.Amount < request.Model.Quantity)
        {
            throw new BusinessException("insufficientStockMsg", param1: product.Name);
        }

        // Sepette bu ürün zaten var mı kontrol et
        var existingItem = basket.BasketItems.FirstOrDefault(i => i.Product.Id == request.Model.ProductId);

        if (existingItem == null)
        {
            // Yeni BasketItem ekle
            var newItem = new BasketItem
            {
                Product = product,
                Amount = request.Model.Quantity,
                TotalCost = product.Price * request.Model.Quantity
            };
            basket.BasketItems.Add(newItem);
        }
        else
        {
            // Mevcut item'e miktar ekle
            existingItem.Amount += request.Model.Quantity;
            existingItem.TotalCost = existingItem.Amount * product.Price;
        }

        // Stok düş
        product.Stock.Amount -= request.Model.Quantity;
        _productRepository.Update(product);

        _basketRepository.Update(basket);
        return true;
    }
}
