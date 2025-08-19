using Microsoft.Extensions.Configuration;

namespace BikeAdm.Domain.Extensions;

public static class ExtensionRepository
{
    public static string GetConnection(this IConfiguration configurationManager)
    {
        var connection = configurationManager.GetConnectionString("Connection");
        return connection;
    }

    public static string GetDatabaseName(this IConfiguration configurationManager)
    {
        var databaseName = configurationManager.GetConnectionString("DatabaseName");
        return databaseName;
    }
}
