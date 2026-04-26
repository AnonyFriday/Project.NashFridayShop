using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NashFridayStore.BFF.Features.Tokens.GetTokens;

public sealed class Handler(
    IWebHostEnvironment environment,
    IHttpContextAccessor httpContextAccessor,
    ILogger<Handler> logger
    )
{
    public async Task<Response> HandleAsync(CancellationToken ct)
    {
        // Only used in Development Environment to check the token
        logger.LogWarning("This api is only used for development");
        if (!environment.IsDevelopment())
        {
            throw new InvalidOperationException("This endpoint is only available in development.");
        }

        HttpContext httpContext = httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext Not Found");
        }

        var response = new Response(
            Claims: httpContext.User.Claims.Select(c => new ClaimResponse(c.Type, c.Value)),
            Tokens: new TokensResponse(
                AccessToken: await httpContext.GetTokenAsync("access_token") ?? string.Empty,
                IdToken: await httpContext.GetTokenAsync("id_token") ?? string.Empty,
                RefreshToken: await httpContext.GetTokenAsync("refresh_token") ?? string.Empty
            )
        );

        return response;
    }
}
