using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.API.Exceptions;

namespace NashFridayStore.API.Features.Categories.GetCategory;

internal static class GetCategoryErrors
{
    internal static RequestValidationException Validation(IList<ValidationFailure> errors)
    {
        return new RequestValidationException(
            errors.Select(e => new RequestValidationError(e.PropertyName, e.ErrorMessage)));
    }

    internal static ApiResponseException NotFound(Guid id)
    {
        return new ApiResponseException(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Category not found.",
            Detail = $"Category with id '{id}' was not found.",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"
        });
    }
}
