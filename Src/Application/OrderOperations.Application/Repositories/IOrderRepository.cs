using OrderOperations.Application.Interfaces;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Repositories;

public interface IOrderRepository : IGenericRepository<Order, Guid> { }
