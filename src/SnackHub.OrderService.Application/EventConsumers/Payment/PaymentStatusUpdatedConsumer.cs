using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Models.Payment;

namespace SnackHub.OrderService.Application.EventConsumers.Payment;

public class PaymentStatusUpdatedConsumer : IConsumer<PaymentStatusUpdated>
{
    private readonly ILogger<PaymentStatusUpdatedConsumer> _logger;
    
    private readonly IPublishEndpoint _publishEndpoint;

    public PaymentStatusUpdatedConsumer(
        ILogger<PaymentStatusUpdatedConsumer> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }
    
    
    public async Task Consume(ConsumeContext<PaymentStatusUpdated> context)
    {
        _logger.LogInformation("Payment Status Update Event Received for order [{orderId}]", context.Message.OrderId);

        switch (context.Message.Status)
        {
            case TransactionState.Approved:
                await _publishEndpoint.Publish(new PaymentApproved(context.Message.OrderId, context.Message.TransactionId));
                break;
            case TransactionState.Refused:
            case TransactionState.Canceled:
            case TransactionState.Rejected:
                await _publishEndpoint.Publish(new PaymentRejected(context.Message.OrderId, context.Message.TransactionId));
                break;
            case TransactionState.Accepted:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}