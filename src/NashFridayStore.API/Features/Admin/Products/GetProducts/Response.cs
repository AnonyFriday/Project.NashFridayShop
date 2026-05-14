using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.API.Features.Admin.Products.GetProducts;

public sealed record ProductItem(Guid Id, string Name, string CategoryName, string ImageUrl, decimal PriceUsd, ProductStatus Status, decimal AverageStars, int Quantity, bool IsDeleted, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc);

public sealed record Response(
    IReadOnlyList<ProductItem> Items,
    int TotalItems,
    int TotalPages,
    int PageIndex);



