using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.Infrastructure.AppOptions;

public sealed record ClientUrlsOption
{
    public const string ClientUrls = "ClientUrls";

    [Required]
    public string[] AdminUrls { get; init; } = [];
}
