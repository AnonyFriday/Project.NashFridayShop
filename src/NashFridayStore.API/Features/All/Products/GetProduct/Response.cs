using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.API.Features.All.Products.GetProduct;

public sealed record Response(
    Guid Id,
    Guid CategoryId,
    string CategoryName,
    string Name,
    string Description,
    string ImageUrl,
    decimal PriceUsd,
    int Quantity,
    ProductStatus Status,
    decimal AverageStars);
