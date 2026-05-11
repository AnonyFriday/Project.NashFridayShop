namespace NashFridayStore.StoreFront.Services.Products;

public enum ProductStatus
{
    Discontinued,
    OutOfStock,
    InStock
}

public enum SortBy
{
    NameDesc,
    NameAsc,
    PriceDesc,
    PriceAsc,
    RatingAsc,
    RatingDesc
}

public static class GetProducts
{
    public record ProductItem(
        Guid Id,
        Guid CategoryId,
        string Name,
        string ImageUrl,
        string Description,
        string CategoryName,
        decimal PriceUsd,
        ProductStatus Status,
        decimal AverageStars,
        bool IsNew,
        int Quantity);

    public record Response(List<ProductItem> Items, int TotalItems, int TotalPages, int PageIndex);

    public record Request(
        string? SearchName,
        Guid? CategoryId,
        decimal? MinPrice,
        decimal? MaxPrice,
        ProductStatus? Status = null,
        SortBy? SortBy = null,
        int PageIndex = 0,
        int PageSize = 10);
}

public static class GetTopRatedProducts
{
    public record ProductItem(
        Guid Id,
        Guid CategoryId,
        string Name,
        string CategoryName,
        string ImageUrl,
        decimal PriceUsd,
        ProductStatus Status,
        decimal AverageStars,
        bool IsNew,
        int Quantity);

    public record Response(List<ProductItem> Items);

    public record Request(int Count = 10);
}

public static class GetProduct
{
    public record Response(
        Guid Id,
        Guid CategoryId,
        string CategoryName,
        string Name,
        string Description,
        string ImageUrl,
        decimal PriceUsd,
        int Quantity,
        ProductStatus Status,
        decimal AverageStars);

    public record Request(Guid Id);
}

public static class GetProductRatings
{
    public record RatingItem(
        int Stars, 
        string? Comment, 
        string CustomerName, 
        string? CustomerAvatarUrl, 
        DateTime CreatedAtUtc);

    public record Response(
        List<RatingItem> Items,
        int TotalItems,
        int TotalPages,
        int PageIndex,
        decimal Average);

    public record Request(
        Guid ProductId,
        int PageIndex = 0,
        int PageSize = 10);
}
