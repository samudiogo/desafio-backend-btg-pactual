using BtgPactual.Application.Ports;

namespace BtgPactual.OrdersApi.Workers;

public class OrderConsumerWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OrderConsumerWorker> _logger;

    public OrderConsumerWorker(IServiceScopeFactory serviceScopeFactory, ILogger<OrderConsumerWorker> logger)
    {
        _scopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OrderConsumerWorker is starting...");
        
        using var scope = _scopeFactory.CreateScope();

        var consumer = scope.ServiceProvider.GetRequiredService<IMessageConsumer>();

        await consumer.StartConsumingAsync(stoppingToken);
    }
}

