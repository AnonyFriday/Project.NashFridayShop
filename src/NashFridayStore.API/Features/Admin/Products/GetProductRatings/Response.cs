namespace NashFridayStore.API.Features.Admin.Products.GetProductRatings;

public sealed record RatingItem(
    int Stars, 
    string? Comment, 
    string CustomerName, 
    string? CustomerAvatarUrl, 
    DateTime CreatedAtUtc);

public sealed record Response(
    IReadOnlyList<RatingItem> Items,
    int TotalItems,
    int TotalPages,
    int PageIndex,
    decimal Average);
