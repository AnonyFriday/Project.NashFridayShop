namespace NashFridayStore.StoreFront.Commons.Exceptions;

public class ApiException(ApiErrorResponse errorResponse) : Exception(errorResponse.Detail ?? errorResponse.Title)
{
    public ApiErrorResponse ErrorResponse { get; } = errorResponse;
}
