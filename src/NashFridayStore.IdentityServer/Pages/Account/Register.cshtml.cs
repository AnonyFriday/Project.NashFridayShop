using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NashFridayStore.IdentityServer.Commons.Exceptions;
using NashFridayStore.IdentityServer.Features.Register;

namespace NashFridayStore.IdentityServer.Pages.Account;

public class Register(Handler handler) : PageModel
{
    [BindProperty]
    public Request Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public string? ErrorMessage { get; set; }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await handler.HandleAsync(Input, HttpContext.RequestAborted);
            return RedirectToPage("Login", new { returnUrl = ReturnUrl });
        }
        catch (Exceptions.ValidationException ex)
        {
            foreach (ValidationFailure error in ex.Errors)
            {
                ModelState.AddModelError($"Input.{error.PropertyName}", error.ErrorMessage);
            }
        }
        catch (Exceptions.IdentityException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (AppException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "An unexpected error occurred during registration.";
        }

        return Page();
    }
}
