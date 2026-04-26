using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.Infrastructure.AppOptions;

public sealed record SiteUrlsOption
{
    public const string SiteUrls = "SiteUrls";

    [Required]
    public string[] AdminUrls { get; init; } = [];
}
