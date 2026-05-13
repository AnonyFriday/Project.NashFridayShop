namespace NashFridayStore.API.Features.Admin.Categories.CreateCategory;

public sealed record Response(
    Guid Id,
    string Name,
    string Description);
