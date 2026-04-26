using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.BFF.AppOptions;

public sealed record ConnectionStringsOptions
{
    public const string ConnectionStrings = "ConnectionStrings";

    [Required]
    public string Database { get; init; } = string.Empty;
}
