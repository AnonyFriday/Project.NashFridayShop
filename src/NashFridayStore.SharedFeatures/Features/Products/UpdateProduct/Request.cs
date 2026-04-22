using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.SharedFeatures.Features.Products.UpdateProduct;

public sealed record RequestBody(
    Guid CategoryId,
    string Name,
    string Description,
    decimal PriceUsd,
    string ImageUrl,
    int Quantity,
    ProductStatus Status);

public sealed record Request(
    Guid ProductId,
    RequestBody RequestBody);
