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
    private readonly IPublishEndpoint _publishEndpoint;

    public PaymentRejectedConsumer(
        ILogger<PaymentRejectedConsumer> logger,
        IOrderRepository orderRepository, 
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
    }
    
    
    public async Task Consume(ConsumeContext<PaymentRejected> context)
    {
        _logger.LogInformation("Payment Rejected Event Received for order [{orderId}]", context.Message.OrderId);
        
        var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
        if (order is null)
        {
            // Do something/Return a event message in case it does not exists
            return;
        }
        
        order.Checkout(false);
        order.UpdateOrderStatus(OrderStatus.Declined);
        await _orderRepository.EditAsync(order);
        
        // Todo: What should happens when a payment is rejected ?
        // Rise a new event ?
        // Nothing since we are already storing the information at the database ?
    }
}