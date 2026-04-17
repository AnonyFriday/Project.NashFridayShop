using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Exceptions;

public class ApiResponseException(ProblemDetails problemDetails) : Exception
{
    public ProblemDetails ProblemDetails { get; } = problemDetails;
    public int StatusCode { get; } = problemDetails.Status!.Value;
}
