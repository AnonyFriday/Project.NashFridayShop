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
        // Only for setting admin-site page, will pass returnUrl from admin-site later
        string defaultReturnUrl = siteUrlsOptions.Value.AdminUrls.FirstOrDefault() ?? "/";

        if (string.IsNullOrWhiteSpace(returnUrl))
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
