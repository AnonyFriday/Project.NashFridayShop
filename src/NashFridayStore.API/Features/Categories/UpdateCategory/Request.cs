namespace NashFridayStore.API.Features.Categories.UpdateCategory;

public sealed record Request(
    Guid Id,
    RequestBody Body);

public sealed record RequestBody(
    string Name,
    string Description);
