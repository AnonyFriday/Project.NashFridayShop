namespace NashFridayStore.API.Features.Products.PostProductRating;

public record Response(Guid ProductId, int Stars, string? Comment, DateTime CreatedAtUtc);
