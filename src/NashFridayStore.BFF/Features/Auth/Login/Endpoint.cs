using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NashFridayStore.BFF.AppOptions;

namespace NashFridayStore.BFF.Features.Auth.Login;

[ApiController]
[Route("api/auth/login")]
public class Endpoint(IOptions<SiteUrlsOption> siteUrlsOptions) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> LoginAsync([FromQuery] string? returnUrl)
    {
        // cannot use Url.IsLocalUrl here due to different origin from bff, admin-site, customer-site
        string[] allowedUrls = siteUrlsOptions.Value.AllowedReturnUrls;
        string defaultReturnUrl = allowedUrls.FirstOrDefault() ?? "/";

        if (string.IsNullOrWhiteSpace(returnUrl) || !allowedUrls.Any(url => returnUrl.StartsWith(url, StringComparison.OrdinalIgnoreCase)))
        {
            returnUrl = defaultReturnUrl;
        }

        await HttpContext.ChallengeAsync(
            OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties
            {
                RedirectUri = returnUrl
            }
        );

        return new EmptyResult();
    }
}
