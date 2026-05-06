using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.StoreFront.AppOptions;

public sealed record ApiUrlOptions
{
    public const string ApiSettings = "ApiSettings";

    [Required]
    public string BaseApiAddress { get; set; } = string.Empty;
}