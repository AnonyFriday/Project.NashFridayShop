using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NashFridayStore.BFF.AppOptions;

namespace NashFridayStore.BFF.Features.Auth.Logout;

[ApiController]
[Route("api/auth/logout")]
public class Endpoint(IOptions<SiteUrlsOption> siteUrlsOptions) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> LogoutAsync([FromQuery] string? returnUrl)
    {
        string[] allowedUrls = siteUrlsOptions.Value.AllowedReturnUrls;
        string defaultReturnUrl = allowedUrls.FirstOrDefault() ?? "/";

        // Validate that the returnUrl is allowed to prevent Open Redirect attacks
        if (string.IsNullOrWhiteSpace(returnUrl) || !allowedUrls.Any(url => returnUrl.StartsWith(url, StringComparison.OrdinalIgnoreCase)))
        {
            returnUrl = defaultReturnUrl;
        }

        // Clear local cookie and trigger OIDC sign-out redirect to Identity Server
        return SignOut(
            new AuthenticationProperties { RedirectUri = returnUrl },
            OpenIdConnectDefaults.AuthenticationScheme,
            CookieAuthenticationDefaults.AuthenticationScheme
        );
    }
}
