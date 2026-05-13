namespace NashFridayStore.StoreFront.Services.Categories;

public static class GetCategories
{
    public record CategoryItem(Guid Id, string Name, string Description);

    public record Request(string? SearchName = null, int? PageIndex = 0, int? PageSize = 10, bool? IsAll = true);
    public record Response(List<CategoryItem> Items, int TotalItems, int TotalPages, int PageIndex);
}
