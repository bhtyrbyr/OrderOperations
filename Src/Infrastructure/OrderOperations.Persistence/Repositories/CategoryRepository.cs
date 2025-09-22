using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.Persistence.Context;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Persistence.Repositories;

public class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}
