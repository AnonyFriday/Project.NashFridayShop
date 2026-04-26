using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.API.Features.Products.CreateProduct;

public sealed record Request(
    Guid CategoryId,
    string Name,
    string Description,
    decimal PriceUsd,
    string ImageUrl,
    int Quantity,
    ProductStatus Status = ProductStatus.InStock);
