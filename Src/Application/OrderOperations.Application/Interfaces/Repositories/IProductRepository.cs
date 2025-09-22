using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product, Guid> { }
