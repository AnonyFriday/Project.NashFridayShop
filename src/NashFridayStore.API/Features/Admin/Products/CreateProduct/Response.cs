using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.API.Features.Admin.Products.CreateProduct;

public sealed record Response(
    Guid Id,
    Guid CategoryId,
    string Name,
    string Description,
    decimal PriceUsd,
    string ImageUrl,
    int Quantity,
    ProductStatus Status,
    DateTime CreatedAtUtc);
