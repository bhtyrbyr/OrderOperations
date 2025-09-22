using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.Persistence.Context;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Persistence.Repositories;

public class OutboxMessageRepository : GenericRepository<OutboxMessage, Guid>, IOutboxMessageRepository
{
    public OutboxMessageRepository(AppDbContext context) : base(context)
    {
    }
}
