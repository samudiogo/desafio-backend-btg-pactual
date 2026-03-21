using BtgPactual.Domain.Entities;

namespace BtgPactual.Domain.Interfaces;

public interface IOrderRepository
{
    Task SaveAsync(Order order);
    Task<IEnumerable<Order>> GetByClienteAsync(int codigoCliente);
    Task<Order?> GetByCodigoPedidoAsync(int codigoPedido);

}
