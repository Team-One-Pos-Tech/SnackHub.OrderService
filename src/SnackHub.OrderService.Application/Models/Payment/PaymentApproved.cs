using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Models.Payment;

[MessageUrn("snack-hub-payments")]
[EntityName("payment-approved")]
public record PaymentApproved(Guid OrderId, Guid TransactionId);