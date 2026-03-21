namespace BtgPactual.Application.Ports;

public interface IMessageConsumer
{
    Task StartConsumingAsync(CancellationToken cancellationToken);
}