using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnackHub.OrderService.Application.Contracts;
using SnackHub.OrderService.Application.Models.Order;
using SnackHub.OrderService.Domain.Contracts;

namespace SnackHub.OrderService.Application.UseCases;

public class ListOrderUseCase : IListOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public ListOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderResponse>> Execute()
    {
        var orders = await _orderRepository.ListAllAsync();
        
        return orders.Select(o => new OrderResponse
        {
            Id = o.Id,
            Items = o.Items.Select(i => (i.ProductName, i.Quantity)).ToList(),
            Status = o.Status.ToString(),
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt
        }).ToList();
    }
}