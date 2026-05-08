using System.Security.Claims;
using NashFridayStore.StoreFront.Commons;
using NashFridayStore.StoreFront.Services.Identity;

namespace NashFridayStore.StoreFront.Middlewares;

public sealed class SetLoggedInUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IAccountApiClient accountApiClient)
    {
        GetUserInfo.Response userInfo = await accountApiClient.GetUserInfoAsync();

        // Will notify unauthenticated user later
        if (!userInfo.IsAuthenticated)
        {
            context.Response.Redirect("/");
            return;
        }

        // Build the ClaimsPrincipal based on the response from /api/auth/me
        // Make the User.Identity.IsAuthenticated works
        // Add claims from GetUserInfo response to the ClaimsPrincipal
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, userInfo.CustomerId!.Value.ToString()),
            new (ClaimTypes.Email, userInfo.Email!),
            new (ClaimTypes.MobilePhone, userInfo.PhoneNumber!),
        };

        foreach (string role in userInfo.Roles ?? [])
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, AppCts.Identity.AuthenticationType);
        context.User = new ClaimsPrincipal(identity);

        await next(context);
    }
}
