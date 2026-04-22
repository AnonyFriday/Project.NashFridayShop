using FluentValidation.Results;
using NashFridayStore.SharedFeatures.Commons.Exceptions;

namespace NashFridayStore.SharedFeatures.Features.Products.GetProducts;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);
}
