namespace NashFridayStore.StoreFront.Pages.Shared.Components.ProductSearchBar;

public sealed class ProductSearchBarVM
{
    public string Placeholder { get; init; }
    public string Name { get; set; } = string.Empty;

    public ProductSearchBarVM(string placeholder = "Type your search...")
    {
        Placeholder = placeholder;
    }
}