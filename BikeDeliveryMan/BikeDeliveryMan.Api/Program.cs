using BikeDeliveryMan.Api.Filters;
using BikeDeliveryMan.Application;
using BikeDeliveryMan.Infrastructure;
using BikeDeliveryMan.Domain.Extensions;
using BikeDeliveryMan.Infrastructure.Configurations;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureDataBase(builder.Services);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilters)));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureDataBase(IServiceCollection services)
{
    var connection = builder.Configuration.GetConnection();
    var databaseName = builder.Configuration.GetDatabaseName();
    var client = new MongoClient(connection);

    Database.ConfigureRepository(client, databaseName, services);

    services.AddSingleton<IMongoClient>(client);
    services.AddSingleton(sp => client.GetDatabase(databaseName));
}

public partial class Program { }