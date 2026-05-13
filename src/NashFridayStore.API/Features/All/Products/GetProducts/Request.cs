using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.API.Features.All.Products.GetProducts;

public sealed record Request(
    Guid? CategoryId,
    string? SearchName,
    decimal? MinPrice,
    decimal? MaxPrice,
    ProductStatus? Status = null,
    SortBy? SortBy = null,
    int PageIndex = 0,
    int PageSize = 10);

public enum SortBy
{
    NameDesc,
    NameAsc,
    PriceDesc,
    PriceAsc,
    RatingAsc,
    RatingDesc
}
