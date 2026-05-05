namespace NashFridayStore.API.Features.Admin.Categories.GetCategories;

public sealed record Request(
    string? SearchName,
    int PageIndex = 0,
    int PageSize = 10,
    bool IsAll = false);
