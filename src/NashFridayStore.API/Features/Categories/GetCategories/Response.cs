namespace NashFridayStore.API.Features.Categories.GetCategories;

public sealed record CategoryItem(Guid Id, string Name, string Description);

public sealed record Response(
    IReadOnlyList<CategoryItem> Items,
    int TotalItems,
    int TotalPages,
    int PageIndex);
