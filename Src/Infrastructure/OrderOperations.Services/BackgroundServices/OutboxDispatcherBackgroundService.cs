using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderOperations.Application.Interfaces.QueueServices;
using OrderOperations.Contracts;
using OrderOperations.Persistence.Context;
using OrderOperations.Services.DTOs;
using OrderPoerations.Domain.Enums;
using System.Text.Json;

namespace OrderOperations.Services.BackgroundServices;

public class OutboxDispatcherBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxDispatcherBackgroundService> _logger;
    private readonly IQueueService _queueService;
    private readonly string _orderQueueName;
    private readonly int _batchSize;
    private readonly int _intervalSeconds;

    public OutboxDispatcherBackgroundService(IServiceProvider serviceProvider, ILogger<OutboxDispatcherBackgroundService> logger, IQueueService queueService, IConfiguration config)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _queueService = queueService;
        _orderQueueName = config["RabbitMq:OrderQueue"] ?? "orders";
        _batchSize = int.Parse(config["Outbox:BatchSize"] ?? "50");
        _intervalSeconds = int.Parse(config["Outbox:IntervalSeconds"] ?? "5");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Dispatcher started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessages(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing outbox messages");
            }

            await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);
        }

        _logger.LogInformation("Outbox Dispatcher stopped.");
    }

    private async Task ProcessOutboxMessages(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var messages = await dbContext.OutboxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.OccurredAt)
            .Take(_batchSize)
            .ToListAsync(cancellationToken);

        if (!messages.Any())
        {
            _logger.LogInformation("No pending Outbux Message found!");
            return;
        }

        foreach (var message in messages)
        {
            try
            {

                var contract = new OutboxMessageContract<Guid>
                {
                    Id = message.Id,
                    Type = message.Type,
                    Content = message.Content
                };

                await _queueService.PublishAsync(contract, _orderQueueName);

                // Başarılıysa işaretle
                message.ProcessedAt = DateTime.UtcNow;
                message.Attempts++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing outbox message {MessageId}", message.Id);
                message.Attempts++;
                message.Error = ex.Message;

                /*
                    Bu alanda gelişmiş hata yönetimi yapılabilir. Eğer outboxmessage birden fazla defa başarısız olursa, bu sipariş başasız olarak işlenir ve iptale geçer.
                 */

                if(message.Attempts > 5)
                {
                    message.ProcessedAt = DateTime.UtcNow;
                    var content = JsonSerializer.Deserialize<OrderCreatedContent>(message.Content);
                    var order = dbContext.Orders.Where(order => order.Id == content.OrderId).FirstOrDefault();
                    order.Status = OrderStatusEnum.Failed;
                    order.Fail = "The order could not be processed.";
                    _logger.LogError("An error occurred while processing the order 5 times. ", message.Id);
                }
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
