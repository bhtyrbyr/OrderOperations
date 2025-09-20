using OrderOperations.Application.Repositories;
using OrderOperations.Persistence.Context;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Persistence.Repositories;

public class BasketRepository : GenericRepository<Basket, Guid>, IBasketRepository
{
    public BasketRepository(AppDbContext context) : base(context)
    {
    }
}
