using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NashFridayStore.StoreFront.Pages.Shared.Components.ProductSearchBar;

namespace NashFridayStore.StoreFront.Pages;

public abstract class BasePageModel : PageModel
{
    public IActionResult OnGetProductSearchBar(ProductSearchBarRequestVM orgReq)
    {
        return ViewComponent("ProductSearchBar", new { orgReq });
    }
}
