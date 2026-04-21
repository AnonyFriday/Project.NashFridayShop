using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.API.Exceptions;

namespace NashFridayStore.API.Features.Products.PostProductRating;

internal static class PostProductRatingErrors
{
    internal static RequestValidationException Validation(IList<ValidationFailure> errors)
    {
        return new RequestValidationException(
            errors.Select(e => new RequestValidationError(e.PropertyName, e.ErrorMessage)));
    }

    public static ApiResponseException ProductNotFound(Guid productId)
    {
        return new ApiResponseException(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Product not found",
            Detail = $"Product with id '{productId}' was not found.",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"
        });
    }
}
