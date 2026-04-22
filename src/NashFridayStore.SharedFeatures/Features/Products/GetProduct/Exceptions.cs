using FluentValidation.Results;
using NashFridayStore.SharedFeatures.Commons.Exceptions;
using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.SharedFeatures.Features.Products.GetProduct;

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
}
