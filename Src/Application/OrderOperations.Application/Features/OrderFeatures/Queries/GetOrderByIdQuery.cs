using AutoMapper;
using MediatR;
using OrderOperations.Application.DTOs.OrderDTOs;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.CustomExceptions.Exceptions.CommonExceptions;

namespace OrderOperations.Application.Features.OrderFeatures.Queries;

public record GetOrderByIdQuery(Guid Person, Guid OrderId) : IRequest<OrderViewModel>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IProductRepository productRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
    }

    public async Task<OrderViewModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var orderItems = await _orderItemRepository.GetAllAsync(cancellationToken);
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        if (orders == null || !orders.Any(order => order.IsActive == true))
        {
            throw new NotFoundException("orderNotFoundMsg", param1: "modulNameMsg*orderModule");
        }

        var personOrders = orders.Where(order => order.PersonId == request.Person && order.Id == request.OrderId && order.IsActive).ToList();
        if (personOrders == null)
        {
            throw new NotFoundException("orderNotFoundMsg", param1: "modulNameMsg*orderModule");
        }

        return _mapper.Map<OrderViewModel>(personOrders);
    }
}