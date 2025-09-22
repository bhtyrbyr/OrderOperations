using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderOperations.Application.Interfaces.Notifications;
using OrderOperations.Application.Interfaces.QueueServices;
using OrderOperations.Persistence.Context;
using OrderOperations.Services.DTOs;
using OrderPoerations.Domain.Entities;
using OrderPoerations.Domain.Enums;
using System.Text.Json;

namespace OrderOperations.Services.BackgroundServices;

public class OrderProcessingWorker : BackgroundService
{
    private readonly ILogger<OrderProcessingWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IQueueService _queueService;
    private readonly string _queueName = "orders";

    public OrderProcessingWorker(ILogger<OrderProcessingWorker> logger, IServiceProvider serviceProvider, IQueueService queueService)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _queueService = queueService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OrderProcessingWorker started, waiting for messages...");

        await _queueService.SubscribeAsync<OrderCreatedEvent>(_queueName, async message =>
        {
            await ProcessMessageAsync(message, stoppingToken);
        });
    }

    private async Task ProcessMessageAsync(OrderCreatedEvent message, CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var notificationService = scope.ServiceProvider.GetRequiredService<IOrderNotificationService>();

        _logger.LogInformation("Processing order message: {MessageId}", message.Id);

        try
        {
            var content = JsonSerializer.Deserialize<OrderCreatedContent>(message.Content);
            if (content is null)
            {
                _logger.LogError("Invalid message content: {Content}", message.Content);
                return;
            }

            var order = await dbContext.Orders
                                        .Include(b => b.Basket)
                                        .ThenInclude(bi => bi.BasketItems)
                                        .ThenInclude(pr => pr.Product)
                                        .ThenInclude(st => st.Stock)
                                        .FirstOrDefaultAsync(o => o.Id == content.OrderId, ct);

            if (order == null)
            {
                _logger.LogWarning("Order not found for ID {OrderId}", content.OrderId);
                return;
            }

            if (order.Status >= OrderStatusEnum.PendingPayment)
            {
                _logger.LogInformation("Order {OrderId} is already processed, skipping...", content.OrderId);
                return;
            }

            /*
            
                Sipariş senaryosunda stokta yeterli ürün olup olmadığı burada kontrol edilmeli fakat süreci hızlı bir şekilde tamamlamak için stoktan düşme 
                işlemi sepet oluşturulurken yapılmıştır. Bu sebeple ek bir stok kontrol mekanizması buraya eklenmemiştir.

             */

            order.Status = OrderStatusEnum.PendingPayment;
            /*
             
              Ödeme senaryosu burada işlenebilir.
            
             */
            await notificationService.SendOrderStatusUpdatedAsync(order.Id, order.Status.ToString(), ct);
            //await Task.Delay(TimeSpan.FromSeconds(15), ct);

            order.Status = OrderStatusEnum.Processing;
            await notificationService.SendOrderStatusUpdatedAsync(order.Id, order.Status.ToString(), ct);
           // await Task.Delay(TimeSpan.FromSeconds(15), ct);
            order.OrderNumber = dbContext.Orders.Max(o => o.OrderNumber) + 1;
            var basket = dbContext.Baskets.Include(b => b.BasketItems).ThenInclude(bi => bi.Product).Where(b => b.Id == order.Basket.Id).First();
            order.OrderItems = new List<OrderItem>();

            foreach(var item in basket.BasketItems)
            {
                var orderItem = new OrderItem
                {
                    Product = item.Product,
                    Price = item.Product.Price,
                    Amount = item.Amount,
                    TotalCost = item.TotalCost
                };
                order.OrderItems.Add(orderItem);
                item.IsActive = false;
            }

            order.Status = OrderStatusEnum.Completed;

            basket.IsActive = false;

            await dbContext.SaveChangesAsync(ct);

            _logger.LogInformation("Order {OrderId} processed successfully.", content.OrderId);

            await notificationService.SendOrderStatusUpdatedAsync(order.Id, order.Status.ToString(), ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing order message {MessageId}", message.Id);
            throw;
        }
    }
}
