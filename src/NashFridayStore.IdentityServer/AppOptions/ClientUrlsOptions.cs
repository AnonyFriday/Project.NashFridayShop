using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.IdentityServer.AppOptions;

public sealed record ClientUrlsOption
{
    public const string ClientUrls = "ClientUrls";

    [Required]
    public string[] AdminSites { get; init; } = [];
}
