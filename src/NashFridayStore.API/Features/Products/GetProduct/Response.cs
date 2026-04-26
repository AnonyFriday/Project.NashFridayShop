using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.API.Features.Products.GetProduct;

public sealed record Response(
    Guid Id,
    string Name,
    string ImageUrl,
    decimal PriceUsd,
    ProductStatus Status,
    decimal AverageStars);
