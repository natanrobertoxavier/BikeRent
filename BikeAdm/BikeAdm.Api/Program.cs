using BikeAdm.Application;
using BikeAdm.Infrastructure;
using BikeAdm.Domain.Extensions;
using BikeAdm.Infrastructure.Configurations;
using MongoDB.Driver;
using BikeAdm.Api.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureDataBase(builder.Services);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilters)));

var app = builder.Build();

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