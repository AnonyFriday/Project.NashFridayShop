using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Commons;
using NashFridayStore.StoreFront.Services.Cart;
using NashFridayStore.StoreFront.Services.Orders;

namespace NashFridayStore.StoreFront.Pages.Checkout;

public class IndexModel(
    ICartApiClient cartApiClient,
    IOrderApiClient orderApiClient) : BasePageModel
{
    public GetCart.Response? Cart { get; private set; }

    [BindProperty]
    public CreateCheckout.Request CheckoutRequest { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await cartApiClient.GetCartAsync();

        if (Cart == null || !Cart.Items.Any())
        {
            return RedirectToPage("/Cart/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Cart = await cartApiClient.GetCartAsync();

        if (Cart == null || !Cart.Items.Any())
        {
            TriggerToast("Your cart is empty.", AppCts.ToastType.Error);
            return RedirectToPage("/Cart/Index");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        CreateCheckout.Response? response = await orderApiClient.CreateCheckoutAsync(CheckoutRequest);

        if (response == null || string.IsNullOrEmpty(response.CheckoutUrl))
        {
            TriggerToast("Failed to create checkout session. Please try again.", AppCts.ToastType.Error);
            return Page();
        }

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            Response.Headers["HX-Redirect"] = response.CheckoutUrl;
            return Content("");
        }

        return Redirect(response.CheckoutUrl);
    }
}
