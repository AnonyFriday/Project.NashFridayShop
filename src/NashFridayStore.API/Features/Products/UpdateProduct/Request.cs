using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.API.Features.Products.UpdateProduct;

public sealed record RequestBody(
    Guid CategoryId,
    string Name,
    string Description,
    decimal PriceUsd,
    int Quantity,
    ProductStatus Status);

public sealed record Request(
    Guid ProductId,
    RequestBody RequestBody,
    bool IncludeDeleted = false);
