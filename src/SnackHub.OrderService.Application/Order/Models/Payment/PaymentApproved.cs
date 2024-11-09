using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.Payment;


[EntityName("payment-approved")]
public record PaymentApproved(Guid OrderId, Guid TransactionId);