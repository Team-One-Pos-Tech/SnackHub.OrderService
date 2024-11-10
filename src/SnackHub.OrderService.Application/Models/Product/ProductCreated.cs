using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Models.Product;

[MessageUrn("snack-hub-products")]
[EntityName("product-created")]
public record ProductCreated(Guid Id, string Name, decimal Price, string Description);