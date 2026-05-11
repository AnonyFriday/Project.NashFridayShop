using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.Infrastructure.AppOptions;

public class StripeOptions
{
    public const string Stripe = "Stripe";

    [Required]
    public string SecretKey { get; set; } = string.Empty;

    [Required]
    public string WebhookSecret { get; set; } = string.Empty;

    [Required]
    public string ReturnSuccessUrl { get; set; } = string.Empty;

    [Required]
    public string ReturnCancelUrl { get; set; } = string.Empty;
}
