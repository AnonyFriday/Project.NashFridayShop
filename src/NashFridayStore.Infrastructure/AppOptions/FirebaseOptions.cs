using System.ComponentModel.DataAnnotations;

namespace NashFridayStore.Infrastructure.AppOptions;

public class FirebaseOptions
{
    public const string Firebase = "Firebase";

    [Required]
    public string Bucket { get; set; } = string.Empty;

    [Required]
    public string ProductImagesFolder { get; set; } = string.Empty;

    [Required]
    public string CustomerImagesFolder { get; set; } = string.Empty;
}
