namespace NashFridayStore.StoreFront.Pages.Shared.Components.CategoryNavigation;

public sealed class CategoryNavigationResponseVM
{
    public record Category(string Id, string Name);
    public IReadOnlyList<Category> Categories { get; init; } = [];
}