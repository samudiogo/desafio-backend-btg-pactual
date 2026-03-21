using BtgPactual.Domain.Entities;
using BtgPactual.Domain.Interfaces;
using BtgPactual.Shared.DTOs;

namespace BtgPactual.Application.UseCases.ProcessOrder;

public class ProcesssOrderUseCase
{
    private readonly IOrderRepository _repository;

    public ProcesssOrderUseCase(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(OrderMessageDto dto)
    {
        var itens = dto.Itens.Select(i => new OrderItem(i.Produto, i.Quantidade, i.Preco)).ToList();

        var order = new Order(dto.CodigoPedido, dto.CodigoCliente, itens);

        await _repository.SaveAsync(order);
    }
}