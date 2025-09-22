using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.Persistence.Context;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Persistence.Repositories;

public class OrderRepository : GenericRepository<Order, Guid>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }
}
