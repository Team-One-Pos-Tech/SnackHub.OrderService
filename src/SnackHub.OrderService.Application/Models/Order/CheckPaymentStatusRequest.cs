using System;

namespace SnackHub.OrderService.Application.Models.Order
{
    public class CheckPaymentStatusRequest
    {
        public required Guid OrderId { get; init; }
    }
}
