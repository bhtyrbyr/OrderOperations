using OrderOperations.Application.Repositories;
using OrderOperations.Persistence.Context;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Persistence.Repositories;

public class BasketItemRepository : GenericRepository<BasketItem, int>, IBasketItemRepository
{
    public BasketItemRepository(AppDbContext context) : base(context)
    {
    }
}
