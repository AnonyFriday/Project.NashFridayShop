namespace NashFridayStore.StoreFront.Commons.Exceptions;

// Mapping with the error response format from the server
public sealed record ApiErrorResponse(
    string Title,
    string Detail,
    int Status,
    IDictionary<string, string[]>? Errors = null);
