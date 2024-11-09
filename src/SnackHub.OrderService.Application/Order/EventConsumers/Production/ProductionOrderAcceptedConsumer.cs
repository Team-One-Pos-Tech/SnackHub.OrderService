using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Order.Models.ProductionOrder;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.ValueObjects;

namespace SnackHub.OrderService.Application.Order.EventConsumers.Production;

public class ProductionOrderAcceptedConsumer : IConsumer<ProductionOrderAccepted>
{
    private readonly ILogger<ProductionOrderAcceptedConsumer> _logger;
    private readonly IOrderRepository _orderRepository;

    public ProductionOrderAcceptedConsumer(
        ILogger<ProductionOrderAcceptedConsumer> logger, 
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<ProductionOrderAccepted> context)
    {
        _logger.LogInformation("A production order for order [{orderId}] was accept!", context.Message.OrderId);
        
        var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
        if (order is null)
        {
            // Do something/Return a event message in case it does not exist
            return;
        }
        
        order.UpdateOrderStatus(OrderStatus.Accepted);
        await _orderRepository.EditAsync(order);
    }
}