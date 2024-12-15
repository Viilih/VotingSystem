using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMQSettings");
        services.AddSingleton(sp =>
            new RabbitMqConnection(
                hostName: rabbitMqSettings["HostName"],
                userName: rabbitMqSettings["UserName"],
                password: rabbitMqSettings["Password"]
            ));
        services.AddScoped<IMessagingService, RabbitMqService>();
        return services;
    }
}