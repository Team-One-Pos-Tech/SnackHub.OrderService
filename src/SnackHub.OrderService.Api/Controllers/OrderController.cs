using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnackHub.OrderService.Api.Extensions;
using SnackHub.OrderService.Application.Order.Contracts;
using SnackHub.OrderService.Application.Order.Models.Order;

namespace SnackHub.OrderService.Api.Controllers;

[ApiController]
[Route("api/[controller]/v1"), Authorize]
public class OrderController : ControllerBase
{
    private readonly IConfirmOrderUseCase _confirmOrderUseCase;
    private readonly ICancelOrderUseCase _cancelOrderUseCase;
    private readonly IListOrderUseCase _listOrderUseCase;
    private readonly ICheckPaymentStatusUseCase _checkPaymentStatusUseCase;

    public OrderController(
        IConfirmOrderUseCase confirmOrderUseCase,
        ICancelOrderUseCase cancelOrderUseCase,
        IListOrderUseCase listOrderUseCase,
        ICheckPaymentStatusUseCase checkPaymentStatusUseCase)
    {
        _confirmOrderUseCase = confirmOrderUseCase;
        _cancelOrderUseCase = cancelOrderUseCase;
        _listOrderUseCase = listOrderUseCase;
        _checkPaymentStatusUseCase = checkPaymentStatusUseCase;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll()
    {
        var orders = await _listOrderUseCase.Execute();
        return Ok(orders);
    }
    
    [HttpPost("Confirm")]
    [ProducesResponseType(typeof(ConfirmOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ConfirmOrderResponse>> Confirm(ConfirmOrderRequest request)
    {
        var response = await _confirmOrderUseCase.Execute(request);

        return response.IsValid
            ? Ok(response) 
            : ValidationProblem(ModelState.AddNotifications(response.Notifications));
    }
    
    [HttpPut("Cancel")]
    [ProducesResponseType(typeof(CancelOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CancelOrderResponse>> Cancel(CancelOrderRequest request)
    {
        var response = await _cancelOrderUseCase.Execute(request);

        return response.IsValid
            ? Ok(response) 
            : ValidationProblem(ModelState.AddNotifications(response.Notifications));
    }
    
    [HttpGet("{orderId:guid}/payment-status")]
    [ProducesResponseType(typeof(CheckPaymentStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CheckPaymentStatusResponse>> CheckPaymentStatus(Guid orderId)
    {
        var request = new CheckPaymentStatusRequest { OrderId = orderId };
        var response = await _checkPaymentStatusUseCase.Execute(request);

        return response.IsValid
            ? Ok(response)
            : NotFound();
    }
    
}