namespace BtgPactual.Shared.Responses;

public class ClientSummaryResponse
{
    public int CodigoCliente { get; set; }
    public decimal QuantidadePedidos{ get; set; }
    public IList<OrderResponse> Pedidos { get; set; } = [];
}