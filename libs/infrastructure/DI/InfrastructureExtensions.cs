using BtgPactual.Application.Ports;
using BtgPactual.Application.UseCases.GetOrdersByClient;
using BtgPactual.Application.UseCases.GetOrderTotal;
using BtgPactual.Application.UseCases.ProcessOrder;
using BtgPactual.Domain.Entities;
using BtgPactual.Domain.Interfaces;
using BtgPactual.Infrastructure.Messaging;
using BtgPactual.Infrastructure.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace BtgPactual.Infrastructure.DI;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();

        AddMongoDbConfig(services, configuration);
        AddRabbitMQConfig(services, configuration);
        AddUseCasesConfig(services);

        return services;
    }

    private static void AddUseCasesConfig(IServiceCollection services)
    {
        services.AddScoped<ProcessOrderUseCase>();
        services.AddScoped<GetOrdersByClientUseCase>();
        services.AddScoped<GetOrderTotalUseCase>();
    }

    private static void AddRabbitMQConfig(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(options => configuration.GetSection("RabbitMQ").Bind(options));

        services.AddScoped<IMessageConsumer, RabbitMqConsumer>();
    }

    private static void AddMongoDbConfig(IServiceCollection services, IConfiguration configuration)
    {
        //mongodb
        services.Configure<MongoDbSettings>(options => configuration.GetSection("MongoDB").Bind(options));
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = configuration.GetSection("MongoDB").Get<MongoDbSettings>()!;
            return new MongoClient(settings.ConnectionString);
        });

        BsonClassMap.RegisterClassMap<Order>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });

        services.AddSingleton<MongoDbContext>(sp =>
        {
            var settings = configuration.GetSection("MongoDB").Get<MongoDbSettings>()!;
            var client = sp.GetRequiredService<IMongoClient>();
            return new MongoDbContext(client, settings.DatabaseName);
        });
    }
}

