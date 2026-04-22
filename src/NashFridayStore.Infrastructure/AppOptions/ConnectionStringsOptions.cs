using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.Infrastructure.AppOptions;

public sealed record ConnectionStringsOptions
{
    public const string ConnectionStrings = "ConnectionStrings";

    [Required]
    public string Database { get; init; } = string.Empty;

    [Required]
    public string Caching { get; init; } = string.Empty;

    [Required]
    public string SqliteTesting { get; set; } = string.Empty;
}
