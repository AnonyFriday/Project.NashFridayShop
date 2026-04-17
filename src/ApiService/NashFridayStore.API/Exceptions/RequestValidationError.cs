namespace NashFridayStore.API.Exceptions;

public sealed record RequestValidationError(string PropertyName, string ErrorMessage);
