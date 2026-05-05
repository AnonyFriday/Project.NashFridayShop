namespace NashFridayStore.API.Features.Products.UpdateProductImage;

public sealed record Response(
    Guid ProductId,
    string ImageUrl,
    DateTime UpdatedAtUtc);
