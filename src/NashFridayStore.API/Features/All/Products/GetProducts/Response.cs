using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.API.Features.All.Products.GetProducts;

public sealed record ProductItem(Guid Id, Guid CategoryId, string Name, string CategoryName, string ImageUrl, decimal PriceUsd, ProductStatus Status, decimal AverageStars, int Quantity, bool IsNew);

public sealed record Response(
    IReadOnlyList<ProductItem> Items,
    int TotalItems,
    int TotalPages,
    int PageIndex);
