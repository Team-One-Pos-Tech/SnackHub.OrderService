namespace SnackHub.OrderService.Domain.ValueObjects;

public enum OrderStatus
{
    Pending = 0,    // Default status
    Cancelled = 1,  // Order was cancelled by the client
    Confirmed = 2,  // Order was confirmed by the store (upon successful payment)
    Declined = 3,   // Order was declined by the store (upon failed payment)
    Accepted = 4,   // Order was accepted by production
    Completed = 5,  // Order was completed by production
}