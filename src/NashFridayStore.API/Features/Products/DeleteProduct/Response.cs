namespace NashFridayStore.API.Features.Products.DeleteProduct;

public sealed record Response(Guid Id, bool IsDeleted, DateTime DeletedAtUtc);
