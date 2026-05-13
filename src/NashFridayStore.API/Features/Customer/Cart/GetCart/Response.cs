namespace NashFridayStore.API.Features.Customer.Cart.GetCart;

public record Response(
    Guid CustomerId,
    string? CustomerName,
    string? DeliveryAddress,
    string Currency,
    decimal TotalPriceInUsd,
    DateTime? UpdatedAtUtc,
    ICollection<ShoppingCartItemResponse> Items);

public record ShoppingCartItemResponse(
    Guid ProductId,
    string ProductName,
    decimal PriceInUsd,
    int Quantity,
    string ImageUrl,
    Guid CategoryId,
    string CategoryName);
