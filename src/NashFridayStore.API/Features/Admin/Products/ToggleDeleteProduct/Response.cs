namespace NashFridayStore.API.Features.Admin.Products.ToggleDeleteProduct;

public sealed record Response(Guid Id, bool IsDeleted, DateTime DeletedAtUtc);
