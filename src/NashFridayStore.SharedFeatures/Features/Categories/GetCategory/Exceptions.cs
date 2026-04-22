using FluentValidation.Results;
using NashFridayStore.SharedFeatures.Commons.Exceptions;
using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.SharedFeatures.Features.Categories.GetCategory;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);

    public sealed class CategoryNotFoundException(Guid id) : AppException(
        ProblemDetailsTypes.NotFound,
        "Category Not Found",
        $"Category with ID '{id}' was not found"
    )
    {
        public Guid CategoryId { get; } = id;
    }
}
