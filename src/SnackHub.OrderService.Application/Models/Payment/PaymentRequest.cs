using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Models.Payment;

[MessageUrn("snack-hub-payments")]
[EntityName("payment-requested")]
public record PaymentRequest(
    Guid OrderId,
    Guid? CustomerId,
    decimal Amount
);