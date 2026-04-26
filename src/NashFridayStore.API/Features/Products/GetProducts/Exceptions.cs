using FluentValidation.Results;
using NashFridayStore.API.Commons.Exceptions;

namespace NashFridayStore.API.Features.Products.GetProducts;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);
}
