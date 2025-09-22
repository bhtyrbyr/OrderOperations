using MediatR;
using OrderOperations.Application.DTOs.BasketDTOs;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.CustomExceptions.Exceptions.CommonExceptions;

namespace OrderOperations.Application.Features.BasketFeatures.Commands;

public record RemoveProductFromBasketCommand(RemoveProductFromBasketViewModel Model) : IRequest<bool>;

public class RemoveProductFromBasketHandler : IRequestHandler<RemoveProductFromBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IBasketItemRepository _basketItemRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IProductRepository _productRepository;

    public RemoveProductFromBasketHandler(IBasketRepository basketRepository, IBasketItemRepository basketItemRepository, IStockRepository stockRepository, IProductRepository productRepository)
    {
        _basketRepository = basketRepository;
        _basketItemRepository = basketItemRepository;
        _stockRepository = stockRepository;
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(RemoveProductFromBasketCommand request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var basketItems = await _basketItemRepository.GetAllAsync(cancellationToken);
        var basket = await _basketRepository.GetByIdAsync(request.Model.BasketId, cancellationToken);
        if (basket == null)
        {
            throw new NotFoundException("basketNotFoundMsg", param1: "modulNameMsg*BasketModule");
        }

        var basketItem = basket.BasketItems.FirstOrDefault(i => i.Id == request.Model.BasketItemId);
        if (basketItem == null)
        {
            throw new NotFoundException("productNotInBasketMsg", param1: "modulNameMsg*BasketModule");
        }

        // Ürünün stok bilgisini geri ekle
        var stocks = await _stockRepository.GetAllAsync(cancellationToken);
        var product = products.Where(product => product.Id == basketItem.Product.Id).First();
        if (product != null && product.Stock != null)
        {
            product.Stock.Amount += basketItem.Amount;
            _productRepository.Update(product);
        }

        // BasketItem'i sepetten kaldır
        basket.BasketItems.Remove(basketItem);
        _basketRepository.Update(basket);

        return true;
    }
}