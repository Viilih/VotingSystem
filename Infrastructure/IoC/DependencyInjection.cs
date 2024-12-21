using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, string connectionString)
    {

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            ));

        services.AddScoped<IVoteRepository, VoteRepository>();
        var rabbitMqSettings = configuration.GetSection("RabbitMQSettings");
        services.AddSingleton(sp =>
            new RabbitMqConnection(
                hostName: rabbitMqSettings["HostName"],
                userName: rabbitMqSettings["UserName"],
                password: rabbitMqSettings["Password"]
            ));
        services.AddScoped<IMessagingService, RabbitMqService>();
  
        services.AddHostedService<RabbitMqConsumerService>();
        return services;
    }
}