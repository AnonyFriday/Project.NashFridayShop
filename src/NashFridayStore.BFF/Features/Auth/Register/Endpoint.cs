using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NashFridayStore.BFF.AppOptions;

namespace NashFridayStore.BFF.Features.Auth.Register;

[ApiController]
[Route("api/auth/register")]
public class Endpoint(IOptions<SiteUrlsOption> siteUrlsOptions) : ControllerBase
{
    [HttpGet]
    public IActionResult Register([FromQuery] string? returnUrl)
    {
        // extra layer to protect url.islocalurl
        string[] allowedUrls = siteUrlsOptions.Value.AllowedReturnUrls;
        string defaultReturnUrl = allowedUrls.FirstOrDefault() ?? "/";

        if (string.IsNullOrWhiteSpace(returnUrl) || !allowedUrls.Any(url => returnUrl.StartsWith(url, StringComparison.OrdinalIgnoreCase)))
        {
            returnUrl = defaultReturnUrl;
        }

        // redirect to teh identity server for the form
        string identityServerUrl = siteUrlsOptions.Value.IdentityServer.Authority;
        string registerUrl = $"{identityServerUrl}/Account/Register?returnUrl={Uri.EscapeDataString(returnUrl)}";

        return Redirect(registerUrl);
    }
}
