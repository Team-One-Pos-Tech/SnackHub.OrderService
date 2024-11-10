using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Models.ProductionOrder;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.ValueObjects;

namespace SnackHub.OrderService.Application.EventConsumers.Production;

public class ProductionOrderCompletedConsumer : IConsumer<ProductionOrderCompleted>
{
    private readonly ILogger<ProductionOrderCompletedConsumer> _logger;
    private readonly IOrderRepository _orderRepository;

    public ProductionOrderCompletedConsumer(
        ILogger<ProductionOrderCompletedConsumer> logger, 
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<ProductionOrderCompleted> context)
    {
        _logger.LogInformation("A production order for order [{orderId}] was completed!", context.Message.OrderId);
        
        var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
        if (order is null)
        {
            // Do something/Return a event message in case it does not exist
            return;
        }
        
        order.UpdateOrderStatus(OrderStatus.Completed);
        await _orderRepository.EditAsync(order);
    }
}