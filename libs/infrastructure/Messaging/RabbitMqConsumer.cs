using System.Text;
using System.Text.Json;
using BtgPactual.Application.Ports;
using BtgPactual.Application.UseCases.ProcessOrder;
using BtgPactual.Shared.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace BtgPactual.Infrastructure.Messaging;

public class RabbitMqConsumer : IMessageConsumer
{
    private readonly RabbitMqSettings _settings;
    private readonly ProcessOrderUseCase _processOrderUseCase;
    private readonly ILogger<RabbitMqConsumer> _logger;

    private const int MaxRetries = 10;
    private static readonly TimeSpan InitialDelay = TimeSpan.FromSeconds(2);

    public RabbitMqConsumer(
        IOptions<RabbitMqSettings> settings,
        ProcessOrderUseCase processOrderUseCase,
        ILogger<RabbitMqConsumer> logger)
    {
        _settings = settings.Value;
        _processOrderUseCase = processOrderUseCase;
        _logger = logger;
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        var connection = await CreateConnectionWithRetryAsync(cancellationToken);

        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _settings.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var dto = JsonSerializer.Deserialize<OrderMessageDto>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dto is not null)
                    await _processOrderUseCase.ExecuteAsync(dto);

                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message. Sending to dead-letter.");
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await channel.BasicConsumeAsync(_settings.Queue, autoAck: false, consumer: consumer);

        _logger.LogInformation("RabbitMQ consumer started on queue '{Queue}'", _settings.Queue);

        await Task.Delay(Timeout.Infinite, cancellationToken);
    }

    private async Task<IConnection> CreateConnectionWithRetryAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        for (int attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                _logger.LogInformation(
                    "Attempting RabbitMQ connection ({Attempt}/{MaxRetries})...",
                    attempt, MaxRetries);

                var connection = await factory.CreateConnectionAsync(cancellationToken);

                _logger.LogInformation("RabbitMQ connection established.");

                return connection;
            }
            catch (BrokerUnreachableException ex) when (attempt < MaxRetries)
            {
                var delay = InitialDelay * Math.Pow(2, attempt - 1);

                _logger.LogWarning(
                    ex,
                    "RabbitMQ unavailable. Retrying in {Delay}s... ({Attempt}/{MaxRetries})",
                    delay.TotalSeconds, attempt, MaxRetries);

                await Task.Delay(delay, cancellationToken);
            }
        }

        throw new BrokerUnreachableException(
            new Exception($"Could not connect to RabbitMQ after {MaxRetries} attempts."));
    }
}