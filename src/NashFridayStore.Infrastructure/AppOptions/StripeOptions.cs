using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.Infrastructure.AppOptions;

public class StripeOptions
{
    public const string Stripe = "Stripe";

    [Required]
    public string SecretKey { get; set; } = string.Empty;
}
