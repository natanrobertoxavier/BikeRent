using BikeDeliveryMan.Domain.Repositories.Contracts.User;
using BikeDeliveryMan.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeDeliveryMan.Infrastructure;

public static class Initializer
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services
            .AddScoped<IDeliveryManWriteOnly, DeliveryManRepository>()
            .AddScoped<IDeliveryManReadOnly, DeliveryManRepository>();
    }
}
