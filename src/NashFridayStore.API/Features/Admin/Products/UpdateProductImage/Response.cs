namespace NashFridayStore.API.Features.Admin.Products.UpdateProductImage;

public sealed record Response(
    Guid ProductId,
    string ImageUrl,
    DateTime UpdatedAtUtc);
