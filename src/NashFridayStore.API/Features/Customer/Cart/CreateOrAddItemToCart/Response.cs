namespace NashFridayStore.API.Features.Customer.Cart.CreateOrAddItemToCart;

public record Response(
    Guid Id,
    string Message = "Cart updated successfully");
