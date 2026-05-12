using NashFridayStore.Domain.Entities.Orders;

namespace NashFridayStore.API.Features.Admin.Orders.GetOrders;

public sealed record Response(
    List<OrderItem> Items,
    int TotalItems,
    int TotalPages,
    int PageIndex);

public sealed record OrderItem(
    Guid Id,
    string CustomerFullName,
    string CustomerEmail,
    string DeliveryAddress,
    string PhoneNumber,
    string Currency,
    decimal TotalPriceInUsd,
    OrderStatus OrderStatus,
    PaymentStatus PaymentStatus,
    DateTime CreatedAtUtc);
