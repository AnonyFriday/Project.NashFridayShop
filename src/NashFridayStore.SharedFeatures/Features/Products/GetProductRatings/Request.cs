namespace NashFridayStore.SharedFeatures.Features.Products.GetProductRatings;

public sealed record Request(
    Guid ProductId,
    int PageIndex = 0,
    int PageSize = 10,
    bool IsDeleted = false);
