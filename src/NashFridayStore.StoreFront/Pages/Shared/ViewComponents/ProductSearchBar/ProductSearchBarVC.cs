namespace NashFridayStore.StoreFront.Pages.Shared.Components.SearchBar;

public class SearchBarViewComponent : ViewComponent
{

    [BindProperty(SupportsGet = true)]
    public ProductSearchBarVM SearchBarVM { get; set; } = new ProductSearchBarVM(
        "Search your product here"
    );

    public IViewComponentResult Invoke()
    {
        return View(SearchBarVM);
    }
}
