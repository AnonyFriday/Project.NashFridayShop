using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.Infrastructure.AppOptions;

public class ConnectionStringsOptions
{
    public const string ConnectionStrings = "ConnectionStrings";

    [Required]
    public string Database { get; set; } = string.Empty;

    [Required]
    public string Caching { get; set; } = string.Empty;
}
