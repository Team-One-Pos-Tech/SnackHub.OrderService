using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.ProductionOrder;

[EntityName("production-order-completed")]
public record ProductionOrderCompleted(Guid OrderId);