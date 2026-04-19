using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.API.Exceptions;

namespace NashFridayStore.API.Features.Products.GetProducts;

internal static class GetProductsErrors
{
    internal static RequestValidationException Validation(IList<ValidationFailure> errors)
    {
        return new RequestValidationException(
            errors.Select(e => new RequestValidationError(e.PropertyName, e.ErrorMessage)));
    }
}

