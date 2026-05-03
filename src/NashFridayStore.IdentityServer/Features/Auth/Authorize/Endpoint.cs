using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore;
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
            string encondedReturnUrl = WebUtility.UrlEncode(returnUrl);
            return Redirect($"/Account/Login?returnUrl={encondedReturnUrl}");
        }

        ApplicationUser? user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Unauthorized();
        }

        // Original request from Server
        OpenIddictRequest? request = HttpContext.GetOpenIddictServerRequest();

        if (request is null)
        {
            return BadRequest("OIDC request not found");
        }

        var identity = new ClaimsIdentity(
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
        );

        // Build Claims
        // Claim is a must to have 
        identity.AddClaims([
            // if dont include, the openiddict raise error
            new Claim(OpenIddictConstants.Claims.Subject, user.Id.ToString())
                .SetDestinations([
                    OpenIddictConstants.Destinations.AccessToken,
                    OpenIddictConstants.Destinations.IdentityToken
                    ])
        ]);

        // If BFF requires scope Email
        if (request.HasScope(
            OpenIddictConstants.Scopes.Email))
        {
            identity.AddClaim(
                new Claim(
                    OpenIddictConstants.Claims.Email,
                    user.Email ?? string.Empty
                )
                .SetDestinations(
                    OpenIddictConstants.Destinations.IdentityToken
                )
            );
        }

        // If BFF requires scope Profile = Role + username
        if (request.HasScope(
           OpenIddictConstants.Scopes.Profile))
        {
            identity.AddClaim(
                new Claim(
                    OpenIddictConstants.Claims.Username,
                    user.UserName ?? string.Empty
                )
                .SetDestinations(
                    OpenIddictConstants.Destinations.IdentityToken
                )
            );

            identity.AddClaim(
                new Claim(
                    OpenIddictConstants.Claims.PhoneNumber,
                    user.PhoneNumber ?? string.Empty
                )
                .SetDestinations(
                    OpenIddictConstants.Destinations.IdentityToken
                )
            );
        }

        // If BFF requires scope api
        if (request.HasScope("api"))
        {
            IList<string> roles =
                await userManager.GetRolesAsync(user);

            foreach (string role in roles)
            {
                identity.AddClaim(
                    new Claim(
                        OpenIddictConstants.Claims.Role,
                        role
                    )
                    .SetDestinations(
                        OpenIddictConstants.Destinations.AccessToken,
                        OpenIddictConstants.Destinations.IdentityToken
                    )
                );
            }
        }

        var principal = new ClaimsPrincipal(identity);
        principal.SetScopes(request.GetScopes());

        // OpenIddict will issue a token
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
