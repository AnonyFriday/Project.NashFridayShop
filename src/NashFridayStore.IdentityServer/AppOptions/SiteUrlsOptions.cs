using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.IdentityServer.AppOptions;

public sealed record SiteUrlsOption
{
    public const string SiteUrls = "SiteUrls";

    public BffOptions Bff { get; init; } = new();

    public OidcDebuggerOptions OidcDebugger { get; init; } = new();
}

public sealed record OidcDebuggerOptions
{
    [Required]
    [Url]
    public string Url { get; init; } = string.Empty;

    [Required]
    public string ClientId { get; init; } = string.Empty;

    [Required]
    public string SignInCallbackUrl { get; init; } = string.Empty;

    [Required]
    public string ApiScope { get; init; } = string.Empty;
}

public sealed record BffOptions
{
    [Required]
    [Url]
    public string Url { get; init; } = string.Empty;

    [Required]
    public string ClientId { get; init; } = string.Empty;

    [Required]
    public string ClientSecret { get; init; } = string.Empty;

    [Required]
    public string SignInCallbackUrl { get; init; } = string.Empty;

    [Required]
    public string SignOutCallbackUrl { get; init; } = string.Empty;

    [Required]
    public string ApiScope { get; init; } = string.Empty;
}
