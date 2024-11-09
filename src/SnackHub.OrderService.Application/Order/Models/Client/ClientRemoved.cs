using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.Client;

[EntityName("client-removed")]
public record ClientRemoved(Guid Identifier);