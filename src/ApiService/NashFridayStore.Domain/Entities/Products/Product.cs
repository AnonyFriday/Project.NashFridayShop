namespace NashFridayStore.Domain.Entities.Products;

public sealed class Product
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public Guid SubCategoryId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal PriceUsd { get; set; }
    public string ImageUrl { get; set; } = default!;
    public int Quantity { get; set; }
    public ProductStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
