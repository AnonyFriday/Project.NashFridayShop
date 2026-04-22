namespace NashFridayStore.SharedFeatures.Features.Categories.GetCategories;

public sealed record Request(
    string? SearchName,
    int PageIndex = 0,
    int PageSize = 10,
    bool IsAll = false);
