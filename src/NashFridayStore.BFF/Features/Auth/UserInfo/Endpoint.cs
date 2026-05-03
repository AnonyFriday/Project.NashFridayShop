using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System.Security.Claims;

namespace NashFridayStore.BFF.Features.Auth.UserInfo;

[ApiController]
[Route("api/auth/me")]
[Authorize]
public class Endpoint : ControllerBase
{
    [HttpGet]
    public IActionResult GetUserInfo()
    {
        // For debugging, you can check exactly what claims are available
        var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

        return Ok(new
        {
            IsAuthenticated = true,
            Email = User.FindFirstValue(OpenIddictConstants.Claims.Email),
            PhoneNumber = User.FindFirstValue(OpenIddictConstants.Claims.PhoneNumber),
            Roles = User.FindAll(OpenIddictConstants.Claims.Role).Select(c => c.Value).ToList(),
            AllClaims = allClaims
        });
    }
}
