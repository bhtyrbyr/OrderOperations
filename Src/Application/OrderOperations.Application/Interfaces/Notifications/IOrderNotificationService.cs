namespace OrderOperations.Application.Interfaces.Notifications;

public interface IOrderNotificationService
{
    Task SendOrderStatusUpdatedAsync(Guid orderId, string status, CancellationToken ct = default);
}
