using Flunt.Notifications;
using SnackHub.OrderService.Domain.ValueObjects;

namespace SnackHub.OrderService.Application.Order.Models.Order
{
    public class CheckPaymentStatusResponse : Notifiable<Notification>
    {
        public OrderStatus? PaymentStatus { get; set; }
    }
}
