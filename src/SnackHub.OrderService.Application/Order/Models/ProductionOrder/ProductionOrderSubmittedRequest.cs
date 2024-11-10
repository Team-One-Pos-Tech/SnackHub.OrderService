using System;
using System.Collections.Generic;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.ProductionOrder;

[MessageUrn("snack-hub-production")]
[EntityName("production-order-submitted")]
public record ProductionOrderSubmittedRequest(Guid OrderId, IEnumerable<ProductionOrderProductDetails> ProductList);

public record ProductionOrderProductDetails(Guid ProductId, int Quantity);