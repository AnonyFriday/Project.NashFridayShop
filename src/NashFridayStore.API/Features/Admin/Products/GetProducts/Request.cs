namespace NashFridayStore.API.Features.Admin.Products.GetProducts;

public sealed record Request(
    Guid? CategoryId,
    string? SearchName,
    decimal? MinPrice,
    decimal? MaxPrice,
    ProductStatus? Status = null,
    SortBy? SortBy = null,
    int PageIndex = 0,
    int PageSize = 10,
    bool IncludeDeleted = false);

public enum SortBy
{
    Newest,
    Oldest,
    PriceAsc,
    PriceDesc,
    NameAsc,
    NameDesc,
    QuantityAsc,
    QuantityDesc
}
