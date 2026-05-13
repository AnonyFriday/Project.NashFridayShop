using System.Security.Claims;
using NashFridayStore.StoreFront.Commons;
using NashFridayStore.StoreFront.Commons.Exceptions;
using NashFridayStore.StoreFront.Services.Identity;

namespace NashFridayStore.StoreFront.Middlewares;

public sealed class SetLoggedInUserMiddleware(RequestDelegate next)
{

#pragma warning disable S125 // Sections of code should not be commented out
    // private static readonly string[] _protectedPaths = ["/Orders", "/Checkout", "/Profile"];
#pragma warning restore S125 // Sections of code should not be commented out

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

        try
        {
            GetUserInfo.Response userInfo = await accountApiClient.GetUserInfoAsync();

            // If not authenicate successfully, just go next
            if (userInfo == null || !userInfo.IsAuthenticated)
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

        // Exceptional case, we catch it here, so that the page is still loading and does not stop the page loading
        // pipeline instead of global exception handler is gonna catch exception and stop loading the page
        catch (ApiException ex) when (ex.ErrorResponse.Status == StatusCodes.Status401Unauthorized)
        {
            // Token is invalid or expired, clear stale cookies
            context.Response.Cookies.Delete(AppCts.Cookie.BffCookieName);
            context.Response.Cookies.Delete(AppCts.Cookie.IdentityCookieName);
            await next(context);
        }
    }

    // private async Task HandleUnauthenticatedAsync(HttpContext context, IAccountApiClient accountApiClient)

#pragma warning disable S125 // Sections of code should not be commented out
    // {
    //     string path = context.Request.Path.Value ?? string.Empty;

    //     bool isProtected = _protectedPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase));

    //     if (isProtected)
    //     {
    //         string returnUrl = $"{path}{context.Request.QueryString}";
    //         context.Response.Redirect(accountApiClient.GetLoginRedirectAddress(returnUrl));
    //         return;
    //     }

    //     await next(context);
    // }
}
#pragma warning restore S125 // Sections of code should not be commented out
