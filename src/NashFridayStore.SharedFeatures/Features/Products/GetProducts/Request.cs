using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.SharedFeatures.Features.Products.GetProducts;

public sealed record Request(
    Guid? CategoryId,
    string? SearchName,
    decimal? MinPrice,
    decimal? MaxPrice,
    int PageIndex = 0,
    int PageSize = 10,
    ProductStatus Status = ProductStatus.InStock,
    bool IncludeDeleted = false);
