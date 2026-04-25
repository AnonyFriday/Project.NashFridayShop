using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.IdentityServer.Domain;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace NashFridayStore.IdentityServer.Features.Auth.Authorize;

[ApiController]
[Route("/connect/authorize")]
public class Endpoint(
    UserManager<ApplicationUser> userManager
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Authorize()
    {
        if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
        {
            string returnUrl = Request.Path + Request.QueryString;
            return Redirect($"/Account/Login?returnUrl={returnUrl}");
        }

        ApplicationUser? user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Unauthorized();
        }

        // Build Claims
        var identity = new ClaimsIdentity(
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
        );

        identity.AddClaims([
            // if dont include, the openiddict raise error
            new Claim(OpenIddictConstants.Claims.Subject, user.Id.ToString())
                .SetDestinations([
                    OpenIddictConstants.Destinations.AccessToken,
                    OpenIddictConstants.Destinations.IdentityToken
                    ]),

            new Claim(OpenIddictConstants.Claims.Email, user.Email ?? string.Empty)
                .SetDestinations(OpenIddictConstants.Destinations.IdentityToken),

            new Claim(OpenIddictConstants.Claims.Username, user.UserName ?? string.Empty)
                .SetDestinations(OpenIddictConstants.Destinations.IdentityToken),

            // If include lieral string then raise error 
            // new Claim(OpenIddictConstants.Claims.Address, user.Address ?? string.Empty)
            new Claim(OpenIddictConstants.Claims.PhoneNumber, user.PhoneNumber ?? string.Empty)
                .SetDestinations(OpenIddictConstants.Destinations.IdentityToken),
        ]);

        IList<string> userRoles = await userManager.GetRolesAsync(user);
        foreach (string role in userRoles)
        {
            identity.AddClaim(
                new Claim(OpenIddictConstants.Claims.Role, role)
                .SetDestinations([
                    OpenIddictConstants.Destinations.AccessToken,
                    OpenIddictConstants.Destinations.IdentityToken
                ])
            );
        }

        var principal = new ClaimsPrincipal(identity);
        principal.SetScopes(
            OpenIddictConstants.Scopes.OpenId,
            OpenIddictConstants.Scopes.Email,
            OpenIddictConstants.Scopes.Profile,
            "api"
        );

        // OpenIddict will issue a token
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
