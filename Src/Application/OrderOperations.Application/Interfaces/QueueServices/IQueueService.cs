using OrderOperations.Contracts;

namespace OrderOperations.Application.Interfaces.QueueServices;

public interface IQueueService
{
    void Publish<T>(T message, string queueName);
    void Subscribe<T>(string queueName, Action<T> onMessage);
}
