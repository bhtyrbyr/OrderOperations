using MediatR;
using OrderOperations.Application.Repositories;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Features.BasketFeatures.Commands;

public record CreateBasketCommand(Guid UserId) : IRequest<Guid>;

public class CreateBasketHandler : IRequestHandler<CreateBasketCommand, Guid>
{
    private readonly IBasketRepository _basketRepository;

    public CreateBasketHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Guid> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = new Basket
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId
        };

        await _basketRepository.CreateAsync(basket, cancellationToken);
        return basket.Id;
    }
}
