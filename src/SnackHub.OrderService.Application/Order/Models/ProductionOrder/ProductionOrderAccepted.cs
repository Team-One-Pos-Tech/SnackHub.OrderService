using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.ProductionOrder;

[MessageUrn("snack-hub-production")]
[EntityName("production-order-accepted")]
public record ProductionOrderAccepted(Guid OrderId);