using FluentValidation.Results;
using NashFridayStore.API.Commons.Exceptions;
using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.API.Features.Customer.Products.PostProductRating;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);

    public sealed class ProductNotFoundException(Guid productId) : AppException(
        ProblemDetailsTypes.NotFound,
        "Product Not Found",
        $"Product with ID '{productId}' was not found"
    );

    public sealed class AlreadyPostRatingException(string productName) : AppException(
        ProblemDetailsTypes.Conflict,
        "Customer already rated the product",
        $"You already rated the product {productName}"
    );
}
