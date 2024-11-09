using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.Client;

[EntityName("client-added")]
public record ClientAdded(Guid Identifier);