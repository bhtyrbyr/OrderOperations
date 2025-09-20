using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderOperations.Application.Repositories;
using OrderOperations.Persistence.Context;
using OrderOperations.Persistence.Repositories;
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

        collection.AddScoped<IBasketRepository, BasketRepository>();
        collection.AddScoped<ICategoryRepository, CategoryRepository>();
        collection.AddScoped<IOrderRepository, OrderRepository>();
        collection.AddScoped<IOrderItemRepository, OrderItemRepository>();
        collection.AddScoped<IProductRepository, ProductRepository>();
        collection.AddScoped<IStockRepository, StockRepository>();

        return services;
    }
}
