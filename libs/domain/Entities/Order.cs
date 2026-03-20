namespace Domain.Entities;
class Order
{
    public int CodigoPedido { get; private set; }
    public int CodigoCliente { get; private set; }

    public IReadOnlyList<OrderItem> Itens { get; private set; }   
    public decimal ValorTotal => Itens.Sum(i => i.ValorTotal);

    public Order(int codigoPedido, int codigoCliente, IReadOnlyList<OrderItem> itens)
    {
        CodigoPedido = codigoPedido;
        CodigoCliente = codigoCliente;
        Itens = itens;
    }
}