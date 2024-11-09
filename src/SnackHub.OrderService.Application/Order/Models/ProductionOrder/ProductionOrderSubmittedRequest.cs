using System;
using System.Collections.Generic;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.ProductionOrder;

[EntityName("production-order-submitted")]
public record ProductionOrderSubmittedRequest(Guid OrderId, IEnumerable<ProductionOrderProductDetails> ProductList);

public record ProductionOrderProductDetails(Guid ProductId, int Quantity);