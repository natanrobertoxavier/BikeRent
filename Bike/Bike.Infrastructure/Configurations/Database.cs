using Microsoft.Extensions.DependencyInjection;
using Bike.Domain.Entities;
using MongoDB.Driver;

namespace Bike.Infrastructure.Configurations;

public class Database
{
    public static void ConfigureRepository(IMongoClient client, string databaseName, Microsoft.Extensions.DependencyInjection.IServiceCollection services)
    {
        var database = client.GetDatabase(databaseName);
        var existingCollections = database.ListCollectionNames().ToList();
        var domainAssembly = typeof(BaseEntity).Assembly;
        var entities = domainAssembly
            .GetTypes()
            .Where(t =>
                typeof(BaseEntity).IsAssignableFrom(t) &&
                t.IsClass &&
                !t.IsAbstract)
            .ToList();

        foreach (var entity in entities)
        {
            var collectionName = entity.Name;

            if (!existingCollections.Contains(collectionName))
                database.CreateCollection(collectionName);

            AddCollectionsDI(services, collectionName, entity, database);
        }
    }

    private static void AddCollectionsDI(IServiceCollection services, string collectionName, Type entity, IMongoDatabase database)
    {
        var collectionType = typeof(IMongoCollection<>).MakeGenericType(entity);

        var collectionInstance = database.GetType()
            .GetMethod("GetCollection", new[] { typeof(string), typeof(MongoCollectionSettings) })!
            .MakeGenericMethod(entity)
            .Invoke(database, new object[] { collectionName, null });

        services.AddScoped(collectionType, sp => collectionInstance!);
    }
}
