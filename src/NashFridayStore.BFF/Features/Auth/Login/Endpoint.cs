using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.BFF.Features.Auth.Login;

[ApiController]
[Route("api/auth/login")]
public class Endpoint : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> LoginAsync(string? returnUrl = "/")
    {
        // Precheck returnUrl to avoid returnUrl sending to the malicious website
        if (Url.IsLocalUrl(returnUrl))
        {
            returnUrl = "/";
        }

        // autoamtically redirect to the /connect/authorize
        await HttpContext.ChallengeAsync(
            OpenIdConnectDefaults.AuthenticationScheme,
            new()
            {
                // url that user wants to go back after login successfully
                RedirectUri = returnUrl
            }
        );

        return new EmptyResult();
    }
}
