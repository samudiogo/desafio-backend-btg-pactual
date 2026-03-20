namespace BtgPactual.Shared.DTOs;

public class OrderItemDto
{
    public string Produto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal Preco { get; set; }
}