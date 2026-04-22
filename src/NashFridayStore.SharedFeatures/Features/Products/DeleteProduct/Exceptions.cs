using FluentValidation.Results;
using NashFridayStore.SharedFeatures.Commons.Exceptions;
using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.SharedFeatures.Features.Products.DeleteProduct;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);

    public sealed class ProductNotFoundException(Guid id) : AppException(
        ProblemDetailsTypes.NotFound,
        "Product Not Found",
        $"Product with ID '{id}' was not found"
    )
    {
        public Guid ProductId { get; } = id;
    }

    public sealed class ProductAlreadyDeletedException(Guid id) : AppException(
        ProblemDetailsTypes.BadRequest,
        "Product Already Deleted",
        $"Product with ID '{id}' was already deleted"
    )
    {
        public Guid ProductId { get; } = id;
    }
}
