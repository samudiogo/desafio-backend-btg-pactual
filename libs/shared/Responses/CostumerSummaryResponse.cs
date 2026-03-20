namespace BtgPactual.Shared.Responses;

public class OrderResponse
{
    public int CodigoCliente { get; set; }
    public int QuantidadePedidos { get; set; }
    public List<OrderResponse> Pedidos { get; set; } = [];
}