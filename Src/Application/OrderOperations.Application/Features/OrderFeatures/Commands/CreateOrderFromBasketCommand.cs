using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderOperations.Application.Interfaces.QueueServices;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.Application.Interfaces.UnitOfWork;
using OrderOperations.Contracts;
using OrderOperations.CustomExceptions.Exceptions.BasketExceptions;
using OrderPoerations.Domain.Entities;
using OrderPoerations.Domain.Enums;

namespace OrderOperations.Application.Features.OrderFeatures.Commands;

public record CreateOrderFromBasketCommand(Guid Person, Guid BasketId, string idempotencyKey) : IRequest<bool>;

public class CreateOrderFromBasketHandler : IRequestHandler<CreateOrderFromBasketCommand, bool>
{
    private readonly IQueueService _queueService;
    private readonly IBasketRepository _basketRepository;
    private readonly IBasketItemRepository _basketItemRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOutboxMessageRepository _outboxMessageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderFromBasketHandler(IQueueService queueService, IBasketRepository basketRepository, IBasketItemRepository basketItemRepository, IOrderRepository orderRepository, IOutboxMessageRepository outboxMessageRepository, IUnitOfWork unitOfWork)
    {
        _queueService = queueService;
        _basketRepository = basketRepository;
        _basketItemRepository = basketItemRepository;
        _orderRepository = orderRepository;
        _outboxMessageRepository = outboxMessageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CreateOrderFromBasketCommand request, CancellationToken cancellationToken)
    {
        var basketItems = await _basketItemRepository.GetAllAsync();
        var basket = await _basketRepository.GetByIdAsync(request.BasketId);    
        if (basket == null || basket.UserId != request.Person || basket.IsActive == false)
            throw new Exception("Basket not found");

        if(basket.BasketItems == null || !basket.BasketItems.Any(basketItems => basketItems.IsActive))
            throw new BusinessException("basketEmptyMsg", param1: basket.Id.ToString());

        var control = await _orderRepository.IsOrderExistByIdempotencyKeyAsync(request.Person, request.idempotencyKey);
        if (control)
            throw new BusinessException("idempotencyKeyAlreadyExitsMsg", param1: basket.Id.ToString());

        var order = new Order
        {
            PersonId = request.Person,
            Basket = basket,
            Status = OrderStatusEnum.Created,
            IdempotencyKey = request.idempotencyKey
        };

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _orderRepository.CreateAsync(order, cancellationToken);

            var outbox = new OutboxMessage
            {
                Type = $"CreateOrderV1",
                Content = System.Text.Json.JsonSerializer.Serialize(new { OrderId = order.Id })
            };

            await _outboxMessageRepository.CreateAsync(outbox, cancellationToken);

            // SaveChanges burada çağrılmaz, commit sırasında otomatik yapılır
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
