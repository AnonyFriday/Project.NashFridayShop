using FluentValidation.Results;
using NashFridayStore.API.Commons.Exceptions;

namespace NashFridayStore.API.Features.All.Categories.GetCategories;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);
}
