using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Models.ProductionOrder;

[MessageUrn("snack-hub-production")]
[EntityName("production-order-completed")]
public record ProductionOrderCompleted(Guid OrderId);