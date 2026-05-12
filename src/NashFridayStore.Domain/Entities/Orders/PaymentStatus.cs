namespace NashFridayStore.Domain.Entities.Orders;

public enum PaymentStatus
{
    Pending, // just created, for future development on 1 customer has multiple pending orders
    Paid,
    Failed,
    Refunded,
}
