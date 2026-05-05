namespace NashFridayStore.API.Features.Products.UpdateProductImage;

public sealed record Request(
    Guid ProductId,
    IFormFile ImageFile,
    bool IncludeDeleted = false
);
