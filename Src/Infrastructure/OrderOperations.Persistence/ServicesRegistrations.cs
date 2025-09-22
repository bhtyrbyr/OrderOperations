using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderOperations.Application.Interfaces.Repositories;
using OrderOperations.Application.Interfaces.UnitOfWork;
using OrderOperations.Persistence.Context;
using OrderOperations.Persistence.Repositories;
using OrderOperations.Persistence.UnitOfWorks;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Persistence;

public static class ServicesRegistrations
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string connectionString)
    {
        var collection = services;

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        collection.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        collection.AddIdentity<Person, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        collection.AddScoped<IUnitOfWork, UnitOfWork>();
        collection.AddScoped<IBasketRepository, BasketRepository>();
        collection.AddScoped<IBasketItemRepository, BasketItemRepository>();
        collection.AddScoped<ICategoryRepository, CategoryRepository>();
        collection.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        collection.AddScoped<IOrderRepository, OrderRepository>();
        collection.AddScoped<IOrderItemRepository, OrderItemRepository>();
        collection.AddScoped<IProductRepository, ProductRepository>();
        collection.AddScoped<IStockRepository, StockRepository>();

        return services;
    }
}
