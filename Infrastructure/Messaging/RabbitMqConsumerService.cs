using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messaging;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly RabbitMqConnection _rabbitMqConnection;
    
    public RabbitMqConsumerService(RabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _rabbitMqConnection.GetConnection().CreateModel();
        channel.QueueDeclare("vote_queue", true, false, false,null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received message: {message}");
            await Task.CompletedTask;
        };
        
        channel.BasicConsume(queue: "vote_queue", autoAck: true, consumer: consumer);
        
        return Task.CompletedTask;
    }
}