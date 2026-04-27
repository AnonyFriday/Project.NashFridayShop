using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NashFridayStore.IdentityServer.Domain;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace NashFridayStore.IdentityServer.Pages.Account;

public class Login(
    SignInManager<ApplicationUser> signInManager
) : PageModel
{
    // Contracts
    public sealed record RequestBodyFormInput
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }

    [BindProperty] public RequestBodyFormInput Input { get; set; } = new();
    [BindProperty] public string? ReturnUrl { get; set; }
    public string? ErrorMessage { get; set; }

    // GET
    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    // POST
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        SignInResult result = await signInManager.PasswordSignInAsync(
            Input.Email,
            Input.Password,
            Input.RememberMe,
            false
        );

        if (!result.Succeeded)
        {
            ErrorMessage = "Invalid email or password.";
            return Page();
        }

        if (Url.IsLocalUrl(returnUrl))
        {
            // avoid https://your-bff.com/login?returnUrl=https://hacker-site.com
            return LocalRedirect(returnUrl);
        }
        else
        {
            return RedirectToPage("/Index");
        }
    }
}
