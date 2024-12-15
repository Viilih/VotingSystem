using Infrastructure.Messaging;

namespace Api.IoC;
using Application.IoC;

public static class DependencyInjection 
{
    public static IServiceCollection AddGeneralServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);
        return services;
    }
}