namespace BtgPactual.Shared.DTOs;
public class OrderMessageDto
{
    public int  CodigoPedido { get; set; }
    public int CodigoCliente { get; set; }
    public List<OrderItemDto> Itens { get; set; } = [];
}