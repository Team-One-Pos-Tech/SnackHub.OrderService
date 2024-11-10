using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Models.Payment;

[MessageUrn("snack-hub-payments")]
[EntityName("payment-rejected")]
public record PaymentRejected(Guid OrderId, Guid TransactionId);