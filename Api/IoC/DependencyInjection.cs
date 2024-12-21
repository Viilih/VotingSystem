using Infrastructure.Messaging;

namespace Api.IoC;
using Application.IoC;

public static class DependencyInjection 
{
    public static IServiceCollection AddGeneralServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Connection string: {connectionString}");
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration, connectionString);
        return services;
    }
}