using BtgPactual.Domain.Interfaces;
using BtgPactual.Shared.Extensions;
using BtgPactual.Shared.Responses;

namespace BtgPactual.Application.UseCases.GetOrderTotal;

public class GetOrderTotalUseCase
{
    private readonly IOrderRepository _repository;

    public GetOrderTotalUseCase(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderResponse?> ExecuteAsync(int codigoPedido)
    {
        var order = await _repository.GetByCodigoPedidoAsync( codigoPedido);
        return order?.ToResponse();
    }
}
