using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;

namespace NashFridayStore.IdentityServer.Features.Connect.Logout;

[AllowAnonymous]
[ApiController]
[Route("/connect/logout")]
public class Endpoint : ControllerBase
{
    [HttpPost, HttpGet]
    public async Task<IActionResult> LogoutAsync()
    {
        // Delete cookie LoginSession
        await HttpContext.SignOutAsync(
            IdentityConstants.ApplicationScheme
        );

        // Logout OIDC session and redirect to /signout-callback-oidc
        return SignOut(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
