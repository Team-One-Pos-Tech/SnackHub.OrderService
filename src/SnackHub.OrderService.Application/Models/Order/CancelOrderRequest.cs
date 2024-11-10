using System;

namespace SnackHub.OrderService.Application.Models.Order;

public class CancelOrderRequest
{
    public required Guid OrderId { get; init; }
}
