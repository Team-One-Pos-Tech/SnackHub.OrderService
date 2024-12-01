using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnackHub.OrderService.Application.Contracts;
using SnackHub.OrderService.Application.Models.Order;
using SnackHub.OrderService.Domain.Contracts;

namespace SnackHub.OrderService.Application.UseCases;

public class GetOrderUseCase : IGetOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderResponse>> Execute()
    {
        var orders = await _orderRepository.ListAllAsync();
        
        return orders.Select(order => new OrderResponse
        {
            Id = order.Id,
            Items = order.Items.Select(orderItem => (orderItem.ProductName, orderItem.Quantity)).ToList(),
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        }).ToList();
    }

    public async Task<OrderResponse> Execute(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        return new OrderResponse
        {
            Id = order.Id,
            Items = order.Items.Select(orderItem => (orderItem.ProductName, orderItem.Quantity)).ToList(),
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}