namespace NashFridayStore.API.Features.Products.ToggleDeleteProduct;

public sealed record Response(Guid Id, bool IsDeleted, DateTime DeletedAtUtc);
