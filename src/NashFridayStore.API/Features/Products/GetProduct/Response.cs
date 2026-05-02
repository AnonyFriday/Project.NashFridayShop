using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.API.Features.Products.GetProduct;

public sealed record Response(
    Guid Id,
    Guid CategoryId,
    string Name,
    string Description,
    string ImageUrl,
    decimal PriceUsd,
    int Quantity,
    ProductStatus Status,
    decimal AverageStars)
{
    public string CategoryName { get; set; }
}
