using System;

namespace SnackHub.OrderService.Application.Order.Models.Order;

public class CancelOrderRequest
{
    public required Guid OrderId { get; init; }
}
