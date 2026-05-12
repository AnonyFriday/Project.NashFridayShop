using NashFridayStore.Domain.Entities.Orders;

namespace NashFridayStore.API.Features.Admin.Orders.GetOrderById;

public sealed record Response(
    Guid Id,
    string CustomerFullName,
    string CustomerEmail,
    string DeliveryAddress,
    string PhoneNumber,
    string Currency,
    decimal TotalPriceInUsd,
    OrderStatus OrderStatus,
    PaymentStatus PaymentStatus,
    DateTime CreatedAtUtc,
    List<OrderItemDetail> Items);

public sealed record OrderItemDetail(
    Guid ProductId,
    string ProductName,
    Guid CategoryId,
    string CategoryName,
    int Quantity,
    decimal ProductUnitPriceInUsd);
