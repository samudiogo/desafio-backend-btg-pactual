using BtgPactual.Domain.Entities;
using BtgPactual.Domain.Interfaces;
using MongoDB.Driver;

namespace BtgPactual.Infrastructure.MongoDB;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _collection;

    public OrderRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<Order>("orders");
    }

    public async Task SaveAsync(Order order)
    {
        await _collection.InsertOneAsync(order);
    }

    public async Task<IEnumerable<Order>> GetByClienteAsync(int codigoCliente)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.CodigoCliente, codigoCliente);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<Order?> GetByCodigoPedidoAsync(int codigoPedido)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.CodigoPedido, codigoPedido);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
