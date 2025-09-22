using Microsoft.EntityFrameworkCore;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.Persistence.Context;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Persistence.Repositories;

public class OrderRepository : GenericRepository<Order, Guid>, IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<bool> IsOrderExistByIdempotencyKeyAsync(Guid personId, string idempotencyKey)
    {
        return _context.Orders.AnyAsync(o => o.PersonId == personId && o.IdempotencyKey == idempotencyKey);
    }
}
