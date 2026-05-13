namespace NashFridayStore.StoreFront.Services.Orders;

public static class CreateCheckout
{
    public sealed record Request(
        string CustomerName,
        string CustomerEmail,
        string DeliveryAddress,
        string PhoneNumber);

    public sealed record Response(string CheckoutUrl);
}

public static class GetMyOrders
{
    public sealed record Response(List<OrderItemResponse> Orders);

    public sealed record OrderItemResponse(
        Guid Id,
        string CustomerFullName,
        string CustomerEmail,
        string DeliveryAddress,
        string PhoneNumber,
        string Currency,
        decimal TotalPriceInUsd,
        string OrderStatus,
        string PaymentStatus,
        DateTime CreatedAtUtc,
        List<OrderItemDetailResponse> OrderItems);

    public sealed record OrderItemDetailResponse(
        Guid ProductId,
        string ProductName,
        Guid CategoryId,
        string CategoryName,
        int Quantity,
        decimal ProductUnitPriceInUsd);
}
