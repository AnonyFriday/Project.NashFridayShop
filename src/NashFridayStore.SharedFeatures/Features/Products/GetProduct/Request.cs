namespace NashFridayStore.SharedFeatures.Features.Products.GetProduct;

public sealed record Request(
    Guid Id,
    bool IncludeDeleted = false);
