using BtgPactual.Domain.Entities;
using BtgPactual.Domain.Interfaces;
using BtgPactual.Shared.DTOs;

namespace BtgPactual.Application.UseCases.ProcessOrder;

public class ProcessOrderUseCase
{
    private readonly IOrderRepository _repository;

    public ProcessOrderUseCase(IOrderRepository repository)
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