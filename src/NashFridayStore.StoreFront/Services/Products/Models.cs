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
        string Name, 
        string ImageUrl, 
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
        string Name, 
        string ImageUrl, 
        decimal PriceUsd, 
        ProductStatus Status, 
        decimal AverageStars, 
        bool IsNew,
        int Quantity);

    public record Response(List<ProductItem> Items);

    public record Request(int Count = 10);
}
