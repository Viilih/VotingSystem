using System.Text;
using System.Text.Json;
using Application.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messaging;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMqConnection _rabbitMqConnection;

    public RabbitMqConsumerService(RabbitMqConnection rabbitMqConnection, IServiceScopeFactory scopeFactory)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _rabbitMqConnection.GetConnection().CreateModel();
        channel.QueueDeclare("vote_queue", true, false, false,null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (sender, ea) =>
        {
            using var scope = _scopeFactory.CreateScope();
            var voteRepository = scope.ServiceProvider.GetService<IVoteRepository>();
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received message: {message}");
            await ProcessMessageWithScope(message, voteRepository);
            
            await Task.CompletedTask;
        };
        
        channel.BasicConsume(queue: "vote_queue", autoAck: true, consumer: consumer);
        
        return Task.CompletedTask;
    }
    
    private async Task ProcessMessageWithScope(string message, IVoteRepository voteRepository)
    {
        var voteMessage = JsonSerializer.Deserialize<VoteRequest>(message);
        
        if (voteMessage == null)
        {
            throw new InvalidOperationException("Failed to deserialize vote message");
        }
        
        await voteRepository.ProcessVote(voteMessage.CandidateId);
 
    }
}