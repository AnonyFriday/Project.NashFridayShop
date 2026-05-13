using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NashFridayStore.StoreFront.Pages.Checkout;

public class SuccessModel : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}
