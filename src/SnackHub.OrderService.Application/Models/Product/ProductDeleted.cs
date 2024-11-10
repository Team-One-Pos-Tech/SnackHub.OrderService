using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Models.Product;

[MessageUrn("snack-hub-products")]
[EntityName("product-deleted")]
public record ProductDeleted(Guid Id);