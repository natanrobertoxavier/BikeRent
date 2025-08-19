using BikeDeliveryMan.Domain.Repositories.Contracts.User;
using BikeDeliveryMan.Domain.Services;
using BikeDeliveryMan.Infrastructure.Repositories;
using BikeDeliveryMan.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeDeliveryMan.Infrastructure;

public static class Initializer
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddServices(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services
            .AddScoped<IDeliveryManWriteOnly, DeliveryManRepository>()
            .AddScoped<IDeliveryManReadOnly, DeliveryManRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IPhotoStorageService, PhotoStorageService>();
    }
}
