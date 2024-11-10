using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.Payment;

[MessageUrn("snack-hub-payments")]
[EntityName("payment-requested")]
public record PaymentRequest(
    Guid OrderId,
    decimal Amount,
    object? Metadata = null
);