using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.SharedFeatures.Features.Products.UpdateProduct;

public sealed record Response(
    Guid Id,
    Guid CategoryId,
    string Name,
    string Description,
    decimal PriceUsd,
    string ImageUrl,
    int Quantity,
    ProductStatus Status,
    DateTime UpdatedAtUtc);
