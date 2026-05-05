namespace NashFridayStore.API.Features.Admin.Products.GetProduct;

public sealed record Request(
    Guid Id,
    bool IncludeDeleted = false);
