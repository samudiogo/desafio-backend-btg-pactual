namespace BtgPactual.Domain.Entities;

public class OrderItem
{
    public string Produto { get; private set; }
    public int Quantidade { get; private set; }
    public decimal Preco { get; private set; }
    public decimal ValorTotal => Quantidade * Preco;

    public OrderItem(string produto, int quantidade, decimal preco)
    {
        Produto = produto;
        Quantidade = quantidade;
        Preco = preco;
    }
}