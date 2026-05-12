using NashFridayStore.Domain.Entities.Orders;

namespace NashFridayStore.API.Features.Customer.Orders.GetMyOrders;

public sealed record Response(List<OrderItemResponse> Orders);

public sealed record OrderItemResponse(
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
    List<OrderItemDetailResponse> OrderItems);

public sealed record OrderItemDetailResponse(
    Guid ProductId,
    string ProductName,
    Guid CategoryId,
    string CategoryName,
    int Quantity,
    decimal ProductUnitPriceInUsd);
