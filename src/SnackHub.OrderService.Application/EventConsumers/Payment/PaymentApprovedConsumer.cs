using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Models.Payment;
using SnackHub.OrderService.Application.Models.ProductionOrder;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.ValueObjects;

namespace SnackHub.OrderService.Application.EventConsumers.Payment;

public class PaymentApprovedConsumer : IConsumer<PaymentApproved>
{
    private readonly ILogger<PaymentApprovedConsumer> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public PaymentApprovedConsumer(
        ILogger<PaymentApprovedConsumer> logger,
        IOrderRepository orderRepository, 
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<PaymentApproved> context)
    {
        _logger.LogInformation("Payment Approved Event Received for order [{orderId}]", context.Message.OrderId);
        
        var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
        if (order is null)
        {
            // Do something/Return a event message in case it does not exist
            return;
        }
        
        order.Checkout(true);
        order.UpdateOrderStatus(OrderStatus.Confirmed);
        await _orderRepository.EditAsync(order);

        var products = order
            .Items
            .Select(item => new ProductionOrderProductDetails(item.ProductId, item.Quantity));
        
        await SubmitProductionOrder(order.Id, products);
    }
    
    private async Task SubmitProductionOrder(Guid orderId, IEnumerable<ProductionOrderProductDetails> productList)
    {
        _logger.LogInformation("Submitting a Production Order for order [{orderId}]", orderId);
        
        var eventMessage = new ProductionOrderSubmittedRequest(orderId, productList);
        await _publishEndpoint.Publish(eventMessage);

    }
}