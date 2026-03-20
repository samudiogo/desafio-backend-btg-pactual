namespace BtgPactual.Shared.Responses;
public class OrderItemResponse
{
    public string Produto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal Preco { get; set; }
    public decimal ValorTotal { get; set; }
}