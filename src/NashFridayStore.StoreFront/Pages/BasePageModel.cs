using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NashFridayStore.StoreFront.Pages.Shared.Components.ProductSearchBar;
using NashFridayStore.StoreFront.Services.Identity;

namespace NashFridayStore.StoreFront.Pages;

public abstract class BasePageModel : PageModel
{
    public bool IsLoggedIn { get; private set; }
    public GetUserInfo.Response? CurrentUser { get; private set; }

    public IActionResult OnGetProductSearchBar(ProductSearchBarRequestVM orgReq)
    {
        return ViewComponent("ProductSearchBar", new { orgReq });
    }
}
