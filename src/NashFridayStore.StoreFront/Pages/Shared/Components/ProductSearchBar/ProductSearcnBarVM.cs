namespace NashFridayStore.StoreFront.Pages.Shared.Components.ProductSearchBar;

public sealed class ProductSearchBarVM
{
    public record Product(string Id, string Name, string CategoryName, string ImageUrl);
    public IReadOnlyList<Product> Products { get; set; } = [];
    public string Placeholder { get; init; } = "Search your product here";
    public string SearchName { get; set; } = string.Empty;
}