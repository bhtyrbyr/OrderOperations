using OrderOperations.Contracts;

namespace OrderOperations.Application.Interfaces.QueueServices;

public interface IQueueService
{
    Task PublishAsync<T>(T message, string queueName);
    Task SubscribeAsync<T>(string queueName, Action<T> onMessage);
}
