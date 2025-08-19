using BikeAdm.Domain.Repositories.Contracts.User;
using BikeAdm.Infrastructure.Repositories.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeAdm.Infrastructure;

public static class Initializer
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services
            .AddScoped<IUserWriteOnly, UserRepository>()
            .AddScoped<IUserReadOnly, UserRepository>();
    }
}
