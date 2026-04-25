using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.IdentityServer.Domain;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace NashFridayStore.IdentityServer.Features.Auth.ExchangeToken;

[ApiController]
[Route("/connect/token")]
public class Endpoint : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ExchangeToken()
    {
        OpenIddictRequest? request = HttpContext.GetOpenIddictServerRequest();
        if (request is null)
        {
            throw new InvalidOperationException("Openiddict request cannot be null");
        }

        // check if it's authroization code flow or not 
        if (!request.IsAuthorizationCodeGrantType())
        {
            throw new InvalidOperationException("Grand type is not supported");
        }

        // Openiddict check the body of request
        AuthenticateResult authenticaticateResult = await HttpContext.AuthenticateAsync(
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
        );

        if (!authenticaticateResult.Succeeded)
        {
            throw new InvalidOperationException("The ticket is not succeed.");
        }

        // Openiddict get the temp Principal stored in the code
        ClaimsPrincipal principal = authenticaticateResult.Principal;
        if (principal == null)
        {
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Send accesstoken, Id token based on destination
        return SignIn(
            principal,
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
        );
    }
}
