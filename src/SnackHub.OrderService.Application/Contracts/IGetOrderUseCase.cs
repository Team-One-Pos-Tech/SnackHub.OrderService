using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnackHub.OrderService.Application.Models.Order;

namespace SnackHub.OrderService.Application.Contracts;

public interface IGetOrderUseCase
{
    Task<IEnumerable<OrderResponse>> Execute();
    Task<OrderResponse> Execute(Guid orderId);
}