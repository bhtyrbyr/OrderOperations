using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Interfaces.Repositories;

public interface IOrderRepository : IGenericRepository<Order, Guid> { }
