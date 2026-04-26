using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.BFF.AppOptions;

public sealed record SiteUrlsOption
{
    public const string SiteUrls = "SiteUrls";

    public IdentityServerOptions IdentityServerOptions { get; init; } = new();
}

public sealed record IdentityServerOptions
{
    [Required]
    public string Authority { get; set; } = string.Empty;

    [Required]
    public string ClientId { get; set; } = string.Empty;

    [Required]
    public string ClientSecret { get; set; } = string.Empty;

    [Required]
    public string SignInCallbackPath { get; set; } = string.Empty;

    [Required]
    public string SignInCallbackUrl { get; set; } = string.Empty;

    [Required]
    public string SignOutCallbackPath { get; set; } = string.Empty;

    [Required]
    public string SignOutCallbackUrl { get; set; } = string.Empty;
}
