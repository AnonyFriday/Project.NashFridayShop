namespace NashFridayStore.Domain.Entities.Carts;

public class ShoppingCart
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public string Currency { get; set; } = "USD";
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    public decimal TotalPriceInUsd => Items.Sum(x => x.PriceInUsd * x.Quantity);
    public DateTime UpdatedAtUtc { get; set; }
}
