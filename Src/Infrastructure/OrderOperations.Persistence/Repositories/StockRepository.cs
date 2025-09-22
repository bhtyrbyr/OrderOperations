using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.Persistence.Context;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Persistence.Repositories;

public class StockRepository : GenericRepository<Stock, long>, IStockRepository
{
    public StockRepository(AppDbContext context) : base(context)
    {
    }
}
