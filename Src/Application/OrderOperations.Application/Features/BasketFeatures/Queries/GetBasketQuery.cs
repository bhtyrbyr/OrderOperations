using AutoMapper;
using MediatR;
using OrderOperations.Application.DTOs.BasketDTOs;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.CustomExceptions.Exceptions.CommonExceptions;

namespace OrderOperations.Application.Features.BasketFeatures.Queries;

public record GetBasketQuery(Guid BasketId, Guid CurrentUserId, bool IsAdmin) : IRequest<BasketViewModel>;

public class GetBasketHandler : IRequestHandler<GetBasketQuery, BasketViewModel>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IBasketItemRepository _basketItemRepository;
    private readonly IMapper _mapper;

    public GetBasketHandler(IBasketRepository basketRepository, IBasketItemRepository basketItemRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _basketItemRepository = basketItemRepository;
        _mapper = mapper;
    }

    public async Task<BasketViewModel> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basketItems = await _basketItemRepository.GetAllAsync(cancellationToken);
        var basket = await _basketRepository.GetByIdAsync(request.BasketId, cancellationToken);
        if (basket == null || basket.IsActive == false)
        {
            throw new NotFoundException("basketNotFoundMsg", param1: "modulNameMsg*BasketModule");
        }

        if (!request.IsAdmin && basket.UserId != request.CurrentUserId)
        {
            throw new UnauthorizedAccessException("unauthorizedBasketAccessMsg");
        }

        return _mapper.Map<BasketViewModel>(basket);
    }
}
