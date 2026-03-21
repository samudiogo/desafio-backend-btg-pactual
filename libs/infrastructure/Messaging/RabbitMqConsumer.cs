using BtgPactual.Application.Ports;
using BtgPactual.Application.UseCases.ProcessOrder;
using BtgPactual.Shared.DTOs;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BtgPactual.Infrastructure.Messaging;

public class RabbitMqConsumer : IMessageConsumer
{
    private readonly RabbitMqSettings _settings;
    private readonly ProcessOrderUseCase _processsOrderUseCase;

    public RabbitMqConsumer(IOptions<RabbitMqSettings> settings, ProcessOrderUseCase processsOrderUseCase)
    {
        _settings = settings.Value;
        _processsOrderUseCase = processsOrderUseCase;
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        using var connection = await factory.CreateConnectionAsync(cancellationToken);
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: _settings.Queue, durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            var dto = JsonSerializer.Deserialize<OrderMessageDto>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if(dto is not null)
                await _processsOrderUseCase.ExecuteAsync(dto);

            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
        };

        await channel.BasicConsumeAsync(queue: _settings.Queue, autoAck: false, consumer: consumer);

        await Task.Delay(Timeout.Infinite, cancellationToken);


    }
}

