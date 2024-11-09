using System;

namespace SnackHub.OrderService.Application.Order.Models.Order
{
    public class CheckPaymentStatusRequest
    {
        public required Guid OrderId { get; init; }
    }
}
