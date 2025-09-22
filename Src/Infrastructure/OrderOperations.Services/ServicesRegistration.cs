using Microsoft.Extensions.DependencyInjection;
using OrderOperations.Services.BackgroundServices;

namespace OrderOperations.Services;

public static class ServicesRegistration
{
    public static IServiceCollection AddOrderOperationsServices(this IServiceCollection services)
    {
        var collection = services;

        collection.AddHostedService<OutboxDispatcherBackgroundService>();
        collection.AddHostedService<OrderProcessingWorker>();

        return collection;
    }
}
