using MediatR;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.CustomExceptions.Exceptions.CommonExceptions;

namespace OrderOperations.Application.Features.BasketFeatures.Commands;

public record DeleteBasketCommand(Guid BasketId) : IRequest<bool>;

public class DeleteBasketHandler : IRequestHandler<DeleteBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IBasketItemRepository _basketItemRepository;

    public DeleteBasketHandler(IBasketRepository basketRepository, IBasketItemRepository basketItemRepository)
    {
        _basketRepository = basketRepository;
        _basketItemRepository = basketItemRepository;
    }

    public async Task<bool> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var basketItems = await _basketItemRepository.GetAllAsync(cancellationToken);
        var basket = await _basketRepository.GetByIdAsync(request.BasketId, cancellationToken);
        if (basket == null || basket.IsActive == false)
        {
            throw new NotFoundException("basketNotFoundMsg", param1: "modulNameMsg*BasketModule");
        }
        _basketRepository.Delete(basket);
        return true;
    }
}
