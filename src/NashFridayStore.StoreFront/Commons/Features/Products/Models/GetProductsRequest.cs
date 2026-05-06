namespace NashFridayStore.StoreFront.Commons.Features.Products.Models;

internal sealed record GetProductsRequest(
    Guid? CategoryId,
    string? SearchName,
    decimal? MinPrice,
    decimal? MaxPrice,
    int PageIndex = 0,
    int PageSize = 10
);
