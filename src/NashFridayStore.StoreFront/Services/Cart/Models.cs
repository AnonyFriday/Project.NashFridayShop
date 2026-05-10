namespace NashFridayStore.StoreFront.Services.Cart;

public static class CreateOrAddItemToCart
{
    public record Request(
        Guid ProductId,
        int Quantity,
        string ProductName,
        string ImageUrl,
        decimal Price);

    public record Response(
        Guid Id,
        string Message = "Cart updated successfully");
}

public static class GetCart
{
    public record CartItem(
        Guid ProductId,
        string ProductName,
        decimal PriceInUsd,
        int Quantity,
        string ImageUrl);

    public record Response(
        Guid CustomerId,
        string? CustomerName,
        string? DeliveryAddress,
        string Currency,
        decimal TotalPriceInUsd,
        DateTime? UpdatedAtUtc,
        ICollection<CartItem> Items);
}
