using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.Infrastructure.AppOptions;

public sealed record SiteUrlsOptions
{
    public const string SiteUrls = "SiteUrls";

    [Required]
    public string[] AdminUrls { get; init; } = [];

    [Required]
    public string[] CustomerUrls { get; init; } = [];
}
