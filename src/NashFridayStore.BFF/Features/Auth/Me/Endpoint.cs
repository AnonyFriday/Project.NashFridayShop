using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace NashFridayStore.BFF.Features.Auth.Me;

[ApiController]
[Route("api/auth/me")]

public class Endpoint : ControllerBase
{
    [HttpGet]
    public IActionResult GetUserInfo()
    {
        // For debugging, you can check exactly what claims are available
#pragma warning disable S125 // Sections of code should not be commented out
        // var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
#pragma warning restore S125 // Sections of code should not be commented out

        return Ok(new
        {
            CustomerId = User.FindFirstValue(OpenIddictConstants.Claims.Subject),
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
            Email = User.FindFirstValue(OpenIddictConstants.Claims.Email),
            PhoneNumber = User.FindFirstValue(OpenIddictConstants.Claims.PhoneNumber),
            Roles = User.FindAll(OpenIddictConstants.Claims.Role).Select(c => c.Value).ToList(),
        });
    }
}
