using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.SharedFeatures.Features.Products.GetProducts;

public sealed record ProductItem(Guid Id, string Name, string ImageUrl, decimal PriceUsd, ProductStatus Status, decimal AverageStars);

public sealed record Response(
    IReadOnlyList<ProductItem> ProductItems,
    int TotalItems,
    int TotalPages,
    int PageIndex);



