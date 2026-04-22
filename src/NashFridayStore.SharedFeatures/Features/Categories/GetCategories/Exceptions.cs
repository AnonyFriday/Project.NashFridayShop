using FluentValidation.Results;
using NashFridayStore.SharedFeatures.Commons.Exceptions;

namespace NashFridayStore.SharedFeatures.Features.Categories.GetCategories;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);
}
