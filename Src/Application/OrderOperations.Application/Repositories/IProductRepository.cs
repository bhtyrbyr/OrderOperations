using OrderOperations.Application.Interfaces;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Repositories;

public interface IProductRepository : IGenericRepository<Product, Guid> { }
