namespace NashFridayStore.API.Features.Categories.UpdateCategory;

public sealed record Response(
    Guid Id,
    string Name,
    string Description);
