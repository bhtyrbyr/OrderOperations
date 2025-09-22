using MediatR;
using OrderOperations.Application.Interfaces.QueueServices;
using OrderOperations.Contracts;

namespace OrderOperations.Application.Features.OrderFeatures.Commands;

public record CreateOrderFromBasketCommand(Guid Person, Guid BasketId) : IRequest<bool>;

public class CreateOrderFromBasketHandler : IRequestHandler<CreateOrderFromBasketCommand, bool>
{
    private readonly IQueueService _queueService;

    public CreateOrderFromBasketHandler(IQueueService queueService)
    {
        _queueService = queueService;
    }

    public async Task<bool> Handle(CreateOrderFromBasketCommand request, CancellationToken cancellationToken)
    {
        var contract = new ConvertBasketToOrderContract
        {
            ProcessorId = request.Person,
            BasketId = request.BasketId,
            PaymentType = PaymentTypeEnum.CreditCard,
            OrderDate = DateTime.UtcNow
        };

        _queueService.Publish(contract, "orderQueue");
        return true;
    }
}
