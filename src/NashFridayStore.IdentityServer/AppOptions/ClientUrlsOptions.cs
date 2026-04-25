using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.IdentityServer.AppOptions;

public sealed record ClientUrlsOption
{
    public const string ClientUrls = "ClientUrls";

    [Required]
    public string[] AdminUrls { get; init; } = [];

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
