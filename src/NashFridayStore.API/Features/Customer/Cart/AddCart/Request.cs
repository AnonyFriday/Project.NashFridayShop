namespace NashFridayStore.API.Features.Customer.Cart.AddCart;

public record Request
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}
