using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Models.Payment;

[MessageUrn("snack-hub-payments")]
[EntityName("payment-status-updated")]
public record PaymentStatusUpdated(Guid OrderId, Guid TransactionId, TransactionState Status);


public enum TransactionState
{
    Accepted,
    Refused,
    Approved,
    Rejected,
    Canceled,
}