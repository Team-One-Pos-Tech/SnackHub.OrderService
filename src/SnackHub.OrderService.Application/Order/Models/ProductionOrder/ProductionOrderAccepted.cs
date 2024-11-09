using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.ProductionOrder;

[EntityName("production-order-accepted")]
public record ProductionOrderAccepted(Guid OrderId);