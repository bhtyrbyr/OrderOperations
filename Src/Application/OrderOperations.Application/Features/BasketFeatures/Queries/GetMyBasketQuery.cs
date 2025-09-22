using AutoMapper;
using MediatR;
using OrderOperations.Application.DTOs.BasketDTOs;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.CustomExceptions.Exceptions.CommonExceptions;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Features.BasketFeatures.Queries;

public record GetMyBasketQuery(Guid Person) : IRequest<List<BasketViewModel>>;

public class GetMyBasketQueryHandler : IRequestHandler<GetMyBasketQuery, List<BasketViewModel>>
{
    private readonly IProductRepository _productRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IBasketItemRepository _basketItemRepository;
    private readonly IMapper _mapper;

    public GetMyBasketQueryHandler(IProductRepository productRepository, IBasketRepository basketRepository, IBasketItemRepository basketItemRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _basketRepository = basketRepository;
        _basketItemRepository = basketItemRepository;
        _mapper = mapper;
    }

    public async Task<List<BasketViewModel>> Handle(GetMyBasketQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var basketItems = await _basketItemRepository.GetAllAsync(cancellationToken);
        var baskets = await _basketRepository.GetAllAsync(cancellationToken);
        if (baskets == null)
        {
            throw new NotFoundException("basketNotFoundMsg", param1: "modulNameMsg*BasketModule");
        }
        
        var personBaskets = baskets.Where(basket => basket.UserId == request.Person).ToList();
        if (personBaskets == null)
        {
            throw new NotFoundException("basketNotFoundMsg", param1: "modulNameMsg*BasketModule");
        }

        return _mapper.Map<List<BasketViewModel>>(personBaskets);
    }
}


