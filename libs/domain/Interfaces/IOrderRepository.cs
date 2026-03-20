namespace BtgPactual.Domain.Interfaces;

public interface IOrderRepository
{
    Task SaveAsync(Order order);
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
    
}
