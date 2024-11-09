using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.Payment;

[EntityName("payment-rejected")]
public record PaymentRejected(Guid OrderId, Guid TransactionId);