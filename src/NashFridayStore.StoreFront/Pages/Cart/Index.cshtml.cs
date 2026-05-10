using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Services.Cart;

namespace NashFridayStore.StoreFront.Pages.Cart;

public class IndexModel(ICartApiClient cartApiClient) : BasePageModel
{
    [BindProperty(SupportsGet = true)]
    public GetCart.Response? Cart { get; private set; }

    [BindProperty]
    public CreateOrAddItemToCart.Request? UpdateQuantityRequest { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await cartApiClient.GetCartAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateQuantityAsync()
    {
        if (UpdateQuantityRequest == null)
        {
            return RedirectToPage();
        }

        await cartApiClient.CreateOrAddItemAsync(UpdateQuantityRequest);

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            return RedirectToPage();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveItemAsync()
    {
        if (UpdateQuantityRequest == null)
        {
            return RedirectToPage();
        }

        await cartApiClient.CreateOrAddItemAsync(UpdateQuantityRequest);

        return RedirectToPage();
    }
}
