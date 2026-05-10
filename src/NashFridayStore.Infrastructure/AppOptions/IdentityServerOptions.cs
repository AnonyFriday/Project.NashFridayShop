namespace NashFridayStore.Infrastructure.AppOptions;

public sealed record IdentityServerOptions
{
    public const string IdentityServer = "IdentityServer";

    public string Authority { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}
