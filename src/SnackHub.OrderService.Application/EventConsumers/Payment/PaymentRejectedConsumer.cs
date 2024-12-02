using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Models.Payment;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.ValueObjects;

namespace SnackHub.OrderService.Application.EventConsumers.Payment;

public class PaymentRejectedConsumer : IConsumer<PaymentRejected>
{
    private readonly ILogger<PaymentRejectedConsumer> _logger;
    
    private readonly IOrderRepository _orderRepository;

    public PaymentRejectedConsumer(
        ILogger<PaymentRejectedConsumer> logger,
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    
    
    public async Task Consume(ConsumeContext<PaymentRejected> context)
    {
        _logger.LogInformation("Payment Rejected Event Received for order [{orderId}]", context.Message.OrderId);
        
        var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
        if (order is null)
            return;
        
        order.Checkout(false);
        await _orderRepository.EditAsync(order);
    }
}