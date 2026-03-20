using BtgPactual.Domain.Entities;
using BtgPactual.Shared.Responses;

namespace BtgPactual.Shared.Extensions;

public static class OrderMappingExtensions
{
    public static OrderResponse ToResponse(this Order order) => new ()
    {
        CodigoPedido = order.CodigoPedido,
        CodigoCliente = order.CodigoCliente,
        ValorTotal = order.ValorTotal,
        Itens = order.Itens.Select(i => new OrderItemResponse
        {
            Produto = i.Produto,
            Quantidade = i.Quantidade,
            Preco = i.Preco,
            ValorTotal = i.ValorTotal
        }).ToList()
    };    
}
