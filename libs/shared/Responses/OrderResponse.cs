namespace BtgPactual.Shared.Responses;

public class OrderResponse
{
    public int CodigoPedido { get; set; }
    public int CodigoCliente { get; set; }

    public decimal ValorTotal { get; set; }
    public List<OrderItemResponse> Itens { get; set; } = [];
}