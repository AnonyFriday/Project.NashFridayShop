using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.API.Features.Products.GetProducts;

public sealed record Request(
    Guid? CategoryId,
    string? SearchName,
    decimal? MinPrice,
    decimal? MaxPrice,
    ProductStatus? Status = null,
    int PageIndex = 0,
    int PageSize = 10,
    bool IncludeDeleted = false);
