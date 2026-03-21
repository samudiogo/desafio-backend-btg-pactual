using System.Collections;
using BtgPactual.Application.UseCases.GetOrdersByClient;
using BtgPactual.Application.UseCases.GetOrderTotal;
using BtgPactual.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
namespace orders_api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{

    private readonly GetOrderTotalUseCase _getOrderTotalUseCase;

    private readonly GetOrdersByClientUseCase _getOrdersByClientUseCase;

    public OrderController( GetOrderTotalUseCase getOrderTotal, GetOrdersByClientUseCase getOrdersByClientUseCase)
    {
        _getOrderTotalUseCase = getOrderTotal;
        _getOrdersByClientUseCase = getOrdersByClientUseCase;
    }

    /// <summary>
    /// Valor total do pedido
    /// </summary>
    /// <param name="codigoPedido"></param>
    /// <returns></returns>
    [HttpGet("{codigoPedido}/total")]
    public async Task<ActionResult<OrderResponse>> GetTotal(int codigoPedido)
    {
        var result = await _getOrderTotalUseCase.ExecuteAsync(codigoPedido);

        return result is null ? NotFound() : Ok(result);
    }


    /// <summary>
    /// Lista de pedidos realizados por cliente
    /// </summary>
    /// <param name="codigoCliente"></param>
    /// <returns></returns>
    [HttpGet("clients/{codigoCliente}")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOridersByClient(int codigoCliente)
    {
        var result = await _getOrdersByClientUseCase.ExecuteAsync(codigoCliente);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Quantidade de pedidos por cliente
    /// </summary>
    /// <param name="codigoCliente"></param>
    /// <returns></returns>
    [HttpGet("clients/{codigoCliente}/count")]
    public async Task<ActionResult<int>> GetOrderCountByClient(int codigoCliente)
    {
        var result = await _getOrdersByClientUseCase.ExecuteAsync(codigoCliente);
        return result is null ? NotFound() : Ok(result.Count());
    }
}