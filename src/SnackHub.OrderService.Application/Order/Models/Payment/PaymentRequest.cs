using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.Payment;

[EntityName("payment-requested")]
public record PaymentRequest(
    Guid OrderId,
    decimal Amount,
    object? Metadata = null
);