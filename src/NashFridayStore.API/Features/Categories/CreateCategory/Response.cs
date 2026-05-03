namespace NashFridayStore.API.Features.Categories.CreateCategory;

public sealed record Response(
    Guid Id,
    string Name,
    string Description);
