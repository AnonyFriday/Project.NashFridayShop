namespace NashFridayStore.API.Features.Products.GetProductRatings;

public sealed record RatingItem(int Stars, string? Comment, DateTime CreatedAtUtc);

public sealed record Response(
    IReadOnlyList<RatingItem> Items,
    int TotalItems,
    int TotalPages,
    int PageIndex,
    decimal Average);
