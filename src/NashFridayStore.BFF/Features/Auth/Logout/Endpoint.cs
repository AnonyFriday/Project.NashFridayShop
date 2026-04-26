using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.BFF.Features.Auth.Logout;

[ApiController]
[Route("api/auth/logout")]
public class Endpoint : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> LogoutAsync()
    {
        // remove cookies
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // trigger the /signout-callback-oidc
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties()
        {
            RedirectUri = "/" // later will change with customer site or admin site url
        });

        return new EmptyResult();
    }
}
