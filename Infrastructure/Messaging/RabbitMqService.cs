using System.Text;
using System.Text.Json;
using Application.Interfaces;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;

public class RabbitMqService : IMessagingService
{
    private readonly RabbitMqConnection _rabbitMqConnection;

    public RabbitMqService(RabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }
    
    public async Task PublishMessageAsync<T>(string queueName, T message)
    {
        try
        {
            using var channel = _rabbitMqConnection.GetConnection().CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            channel.BasicPublish(exchange: string.Empty, routingKey: queueName, body: body);

            await Task.CompletedTask; // Keeps the method async for extensibility
        }
        catch (Exception ex)
        {
            // Log and handle the exception (e.g., using ILogger)
            Console.WriteLine($"Error publishing message: {ex.Message}");
            throw;
        }
    }
}