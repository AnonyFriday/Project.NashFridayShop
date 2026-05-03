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
        string defaultReturnUrl = siteUrlsOptions.Value.AdminUrls.FirstOrDefault() ?? "/";

        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            returnUrl = defaultReturnUrl;
        }

        // automatically redirect to the /connect/authorize
        await HttpContext.ChallengeAsync(
            OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties
            {
                // url that user wants to go back after login successfully
                RedirectUri = returnUrl
            }
        );

        return new EmptyResult();
    }
}
