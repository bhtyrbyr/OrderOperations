using OrderOperations.Application.Interfaces.QueueServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrderOperations.Queue.Queue;

public class QueueService : IQueueService
{
    private IConnection? _connection;
    private IChannel? _channel;

    public bool ConnectionState => _connection?.IsOpen ?? false;

    public async Task Initialize(string userName, string userPassword, string HostName, int Port)
    {
        var factory = new ConnectionFactory()
        {
            HostName = HostName,
            Port = Port,
            UserName = userName,
            Password = userPassword
        };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

    }
    public async Task PublishAsync<T>(T message, string queueName)
    {
        if (_channel == null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized.");

        await _channel.QueueDeclareAsync(queue: queueName, 
                                                    durable: false, 
                                                    exclusive: false, 
                                                    autoDelete: false,
                                                    arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
        };
        properties.Persistent = true;

        await _channel.BasicPublishAsync(exchange: string.Empty, 
                                                routingKey: queueName, 
                                                mandatory: false, 
                                                basicProperties: properties, 
                                                body: body);

        //Console.WriteLine($" [x] Sent {message}");
    }

    public async Task SubscribeAsync<T>(string queueName, Action<T> onMessage)
    {
        if (_channel is null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized.");


        await _channel.QueueDeclareAsync(queue: queueName,
                                                    durable: false,
                                                    exclusive: false,
                                                    autoDelete: false,
                                                    arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(json);

                if (message is not null)
                {
                    onMessage(message);
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch
            {
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                throw;
            }
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}