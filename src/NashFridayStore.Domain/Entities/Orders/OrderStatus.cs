namespace NashFridayStore.Domain.Entities.Orders;

public enum OrderStatus
{
    Pending, // just created, for future development on 1 customer has multiple pending orders
    Completed,
    Delivered,
    Cancelled,
    Refunded
}


