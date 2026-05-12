using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using NashFridayStore.StoreFront.Commons;
using NashFridayStore.StoreFront.Commons.Exceptions;

namespace NashFridayStore.StoreFront.ExceptionHandlers;

public class ApiExceptionHandler : IExceptionHandler
{
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ApiException apiException)
        {
            return false;
        }

        ApiErrorResponse error = apiException.ErrorResponse;
        string message = error.Detail ?? error.Title;

        // If having any validation error from api
        if (error.Errors != null && error.Errors.Any())
        {
            KeyValuePair<string, string[]> firstError = error.Errors.First();
            string? firstErrorMessage = firstError.Value.FirstOrDefault();
            if (!string.IsNullOrEmpty(firstErrorMessage))
            {
                message = firstErrorMessage;
            }
        }

        // trigger a toast if we got an error
        var toastEvent = new { message, type = AppCts.ToastType.Error };
        var trigger = new Dictionary<string, object> { { "show-toast", toastEvent } };
        httpContext.Response.Headers["HX-Trigger"] = JsonSerializer.Serialize(trigger, _jsonOptions);

        if (httpContext.Request.Headers.ContainsKey("HX-Request"))
        {
            // return 204 to fix the HTMX issue
            httpContext.Response.StatusCode = StatusCodes.Status204NoContent;
            await httpContext.Response.CompleteAsync();
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsync(message, cancellationToken);
        }

        return true;
    }
}
