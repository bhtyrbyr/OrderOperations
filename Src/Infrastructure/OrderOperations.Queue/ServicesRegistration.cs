using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderOperations.Application.Interfaces.QueueServices;
using OrderOperations.Queue.Queue;

namespace OrderOperations.Queue;

public static class ServicesRegistration
{
    public static IServiceCollection AddQueueServices(this IServiceCollection services, IConfiguration configuration)
    {
        var collection = services;
        collection.AddSingleton<IQueueService>(sp =>
        {
            var queueService = new QueueService();

            var hostName =configuration["RabbitMq:HostName"];
            var port = int.Parse(configuration["RabbitMq:Port"]);
            var userName = configuration["RabbitMq:UserName"];
            var password = configuration["RabbitMq:Password"];

            // Initialize işlemi burada yapılır ve senkron beklenir
            queueService.Initialize(userName, password, hostName, port).GetAwaiter().GetResult();

            return queueService;
        });
        return collection;
    }
}
