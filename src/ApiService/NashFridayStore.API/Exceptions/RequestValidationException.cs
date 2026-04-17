namespace NashFridayStore.API.Exceptions;

public class RequestValidationException(IEnumerable<RequestValidationError> errors) : Exception
{
    public IEnumerable<RequestValidationError> Errors { get; } = errors;
}
