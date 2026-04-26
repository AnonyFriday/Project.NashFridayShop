using FluentValidation.Results;

namespace NashFridayStore.API.Commons.Exceptions;

public class AppValidationException : Exception
{
    public IEnumerable<ValidationFailure> Errors { get; }
    public AppValidationException(IEnumerable<ValidationFailure> errors)
    {
        Errors = errors;
    }
}
