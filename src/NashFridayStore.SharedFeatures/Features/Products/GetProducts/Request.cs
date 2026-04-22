using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.SharedFeatures.Features.Products.GetProducts;

public sealed record ProductItem(Guid Id, string Name, string ImageUrl, decimal PriceUsd, ProductStatus Status, decimal AverageStars);

public sealed record Request(
    Guid? CategoryId,
    string? SearchName,
    decimal? MinPrice,
    decimal? MaxPrice,
    int PageIndex = 0,
    int PageSize = 10,
    ProductStatus Status = ProductStatus.InStock,
    bool IsDeleted = false);

public sealed record Response(
    IReadOnlyList<ProductItem> ProductItems,
    int TotalItems,
    int TotalPages,
    int PageIndex);
