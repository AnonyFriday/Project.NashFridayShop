using FluentValidation.Results;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.SharedFeatures.Commons.Exceptions;

public class AppValidationException : AppException
{
    public IEnumerable<ValidationFailure> Errors { get; }
    public AppValidationException(IEnumerable<ValidationFailure> errors) : base(
        AppCts.ProblemDetailsTypes.BadRequest,
        "Validation failed",
        "See errors for details"
    )
    {
        Errors = errors;
    }
}
