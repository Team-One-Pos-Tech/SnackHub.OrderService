using System;
using Flunt.Notifications;

namespace SnackHub.OrderService.Application.Models.Order;

public class CancelOrderResponse : Notifiable<Notification>
{
    public DateTime? CancelledAt { get; set; }
}