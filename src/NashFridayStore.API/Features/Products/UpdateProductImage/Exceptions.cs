using FluentValidation.Results;
using NashFridayStore.API.Commons.Exceptions;
using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.API.Features.Products.UpdateProductImage;

public static class Exceptions
{
    public static readonly string[] SupportedImageExtensions = { ".jpg", ".jpeg", ".png" };
    public static readonly int MaxImageSize = 5 * 1024 * 1024; 
    public static readonly int MaxImageSizeInMB = 5; 

    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);

    public sealed class ProductNotFoundException(Guid id) : AppException(
        ProblemDetailsTypes.NotFound,
        "Product Not Found",
        $"Product with ID '{id}' was not found"
    );

     public sealed class UnsupportedImageExtensionException(string extension) : AppException(
        ProblemDetailsTypes.UnsupportedMediaType,
        "Unsupported Image Extension",
        $"The file extension '{extension}' is not supported. Supported extensions are: {string.Join(", ", SupportedImageExtensions)}"
    );

    public sealed class ImageTooLargeException(long sizeInBytes) : AppException(
        ProblemDetailsTypes.FileTooLarge,
        "Image Too Large",
        $"The image size ({sizeInBytes / 1024.0 / 1024.0:F2} MB) exceeds the {MaxImageSizeInMB}MB limit."
    );
}