using Microsoft.AspNetCore.SignalR;
using OrderOperations.Application.Interfaces.Notifications;

namespace OrderOperations.WebApi.Notifications;

public class SignalROrderNotificationService : IOrderNotificationService
{
    private readonly IHubContext<Hubs.OrderStatusHub> _hubContext;

    public SignalROrderNotificationService(IHubContext<Hubs.OrderStatusHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendOrderStatusUpdatedAsync(Guid orderId, string status, CancellationToken ct = default)
    {
        await _hubContext.Clients.Group(orderId.ToString())
            .SendAsync("OrderStatusUpdated", new
            {
                orderId,
                status
            }, ct);
    }
}
