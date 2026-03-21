using BtgPactual.Domain.Interfaces;
using BtgPactual.Shared.Extensions;
using BtgPactual.Shared.Responses;

namespace BtgPactual.Application.UseCases.GetOrdersByClient;

public class GetOrdersByClientUseCase
{
    private readonly IOrderRepository _repository;

    public GetOrdersByClientUseCase(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderResponse>> ExecuteAsync(int codigoCliente)
    {
        var orders = await _repository.GetByClienteAsync(codigoCliente);

        return orders.Select(o => o.ToResponse());
    }
}