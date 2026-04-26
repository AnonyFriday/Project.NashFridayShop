namespace NashFridayStore.API.Features.Products.GetProduct;

public sealed record Request(
    Guid Id,
    bool IncludeDeleted = false);
