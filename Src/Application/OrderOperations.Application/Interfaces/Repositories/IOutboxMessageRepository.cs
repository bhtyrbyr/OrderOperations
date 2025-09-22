using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Interfaces.Repositories;

public interface IOutboxMessageRepository : IGenericRepository<OutboxMessage, Guid>
{
}
