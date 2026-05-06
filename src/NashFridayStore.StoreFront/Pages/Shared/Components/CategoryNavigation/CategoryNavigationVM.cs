namespace NashFridayStore.StoreFront.Pages.Shared.Components.CategoryNavigation;

public sealed class CategoryNavigationVM
{
    public record Category(string Id, string Name);
    public IReadOnlyList<Category> Categories { get; set; } = [];
}