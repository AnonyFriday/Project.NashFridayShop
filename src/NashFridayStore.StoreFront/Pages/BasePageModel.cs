using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NashFridayStore.StoreFront.Commons;
using NashFridayStore.StoreFront.Commons.Exceptions;
using NashFridayStore.StoreFront.Pages.Shared.Components.ProductSearchBar;

namespace NashFridayStore.StoreFront.Pages;

public abstract class BasePageModel : PageModel
{
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public IActionResult OnGetProductSearchBar(ProductSearchBarRequestVM orgReq)
    {
        return ViewComponent("ProductSearchBar", new { orgReq });
    }

    public IActionResult OnGetCartStatus()
    {
        return Partial("~/Pages/Shared/PartialViews/Navbar/_CartStatus.cshtml");
    }

    protected void TriggerToast(string message, string type = AppCts.ToastType.Success)
    {
        AppendTrigger("show-toast", new { message, type });
    }

    protected void TriggerCartUpdateIcon()
    {
        AppendTrigger("cart-updated", null);
    }

    private void AppendTrigger(string eventName, object? data)
    {
        // set the key HX-Triggers to the httpContext temporarily within this scope
        Dictionary<string, object?> triggers = HttpContext.Items["HX-Triggers"] as Dictionary<string, object?> ?? new();

        triggers[eventName] = data;
        HttpContext.Items["HX-Triggers"] = triggers; // rewrite back to the httpContext

        // Set the header as a single JSON object (since, the traidtiona append at each trigger will rewrite the hx-trigger)
        // using this technqiue, we maintain a whole hx-trigger as a json object)
        Response.Headers["HX-Trigger"] = JsonSerializer.Serialize(triggers, _jsonOptions);
    }
}
