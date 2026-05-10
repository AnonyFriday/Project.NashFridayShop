namespace NashFridayStore.API.Features.Customer.Cart.CreateOrAddItemToCart;

public record Request(
    Guid ProductId,
    int Quantity,
    string ProductName,
    string ImageUrl,
    decimal Price);
