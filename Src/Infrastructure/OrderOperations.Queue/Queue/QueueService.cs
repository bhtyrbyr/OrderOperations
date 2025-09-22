using OrderOperations.Application.Interfaces.QueueServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrderOperations.Queue.Queue;

public class QueueService : IQueueService
{
    private IConnection connection;
    private IChannel channel;

    public bool connectionState = false;

    public QueueService()
    {
    }

    public async Task Initialize(string userName, string userPassword, string HostName, int Port)
    {
        var factory = new ConnectionFactory()
        {
            HostName = HostName,
            Port = Port,
            UserName = userName,
            Password = userPassword
        };
        connection = await factory.CreateConnectionAsync();
        channel = await connection.CreateChannelAsync();
        connectionState = connection.IsOpen;
    }
    public async void Publish<T>(T message, string queueName)
    {
        await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
        };
        properties.Persistent = true;

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, mandatory: false, basicProperties: properties, body: body);
        //Console.WriteLine($" [x] Sent {message}");
    }

    public async void Subscribe<T>(string queueName, Action<T> onMessage)
    {
        await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(json);

            if (message is not null)
            {
                onMessage(message);
            }

            channel.BasicAckAsync(ea.DeliveryTag, false);
            //Console.WriteLine($" [x] Received {message}");
            return Task.CompletedTask;
        };

        channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
    }
}