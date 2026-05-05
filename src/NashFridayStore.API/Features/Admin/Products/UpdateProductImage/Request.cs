namespace NashFridayStore.API.Features.Admin.Products.UpdateProductImage;

public sealed record Request(
    Guid ProductId,
    IFormFile ImageFile,
    bool IncludeDeleted = false
);
