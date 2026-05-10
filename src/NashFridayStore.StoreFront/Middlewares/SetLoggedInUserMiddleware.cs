using System.Security.Claims;
using NashFridayStore.StoreFront.Commons;
using NashFridayStore.StoreFront.Services.Identity;

namespace NashFridayStore.StoreFront.Middlewares;

public sealed class SetLoggedInUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IAccountApiClient accountApiClient)
    {
        // Logged-in = have those two cookies
        string? bffCookie = context.Request.Cookies[AppCts.Cookie.BffCookieName];
        string? identityCookie = context.Request.Cookies[AppCts.Cookie.IdentityCookieName];

        if (string.IsNullOrEmpty(bffCookie) || string.IsNullOrEmpty(identityCookie))
        {
            await next(context);
            return;
        }

        // if already logged in, pass the authentication
        if (context.User?.Identity?.IsAuthenticated ?? false)
        {
            await next(context);
            return;
        }

        GetUserInfo.Response userInfo = await accountApiClient.GetUserInfoAsync();

        if (!userInfo.IsAuthenticated)
        {
            await next(context);
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
