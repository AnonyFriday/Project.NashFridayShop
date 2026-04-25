using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;

namespace NashFridayStore.IdentityServer.Features.Auth.Authorize;

[ApiController]
[Route("/connect/authorize")]
public class Endpoint : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Redirect("/Account/Login");
        }

        // OpenIddict will issue a token
        return SignIn(User, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
