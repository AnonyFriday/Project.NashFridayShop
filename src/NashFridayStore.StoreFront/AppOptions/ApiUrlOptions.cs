using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.StoreFront.AppOptions;

public sealed record ApiUrlOptions
{
    public const string ApiSettings = "ApiSettings";

    [Required]
    public string BaseApiUrl { get; set; } = string.Empty;
    public string RedirectedLoginUrl { get; set; } = string.Empty;
    public string RedirectedLogoutUrl { get; set; } = string.Empty;
    public string RedirectedRegisterUrl { get; set; } = string.Empty;
    public string StorefrontUrl { get; set; } = string.Empty;
}