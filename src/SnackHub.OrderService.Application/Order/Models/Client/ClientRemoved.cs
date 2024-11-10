using System;
using MassTransit;

namespace SnackHub.OrderService.Application.Order.Models.Client;

[MessageUrn("snack-hub-clients")]
[EntityName("client-removed")]
public record ClientRemoved(Guid Identifier);