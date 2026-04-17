using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.API.Exceptions;

namespace NashFridayStore.API.ExceptionHandlers;

internal sealed class GeneralExceptionHandler(
    ILogger<GeneralExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            // Handle text related exception only
            case ApiResponseException apiResponseException:
                {
                    logger.LogError(exception, "Handle : {Message}", exception.Message);
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = apiResponseException.StatusCode;
                    await httpContext.Response.WriteAsJsonAsync(apiResponseException.ProblemDetails, cancellationToken: cancellationToken);
                    break;
                }

            // Handle only validation errors
            case RequestValidationException requestValidationException:
                {
                    logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "One or more validation errors occurred.",
                        Detail = "See the errors property for details.",
                        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                        Extensions =
                        {
                            ["Errors"] = requestValidationException.Errors
                                .GroupBy(e => e.PropertyName,
                                    (name, errorGroups) => new
                                    {
                                        Field = name,
                                        Messages = errorGroups.Select(x => x.ErrorMessage).ToArray()
                                    })
                        }
                    };

                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
                    break;
                }
            default:
                return false;
        }

        return true;
    }
}
