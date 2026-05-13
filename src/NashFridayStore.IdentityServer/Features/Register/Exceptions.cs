using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using NashFridayStore.IdentityServer.Commons;
using NashFridayStore.IdentityServer.Commons.Exceptions;

namespace NashFridayStore.IdentityServer.Features.Register;

public static class Exceptions
{
    public class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);

    public class IdentityException(IEnumerable<IdentityError> errors) : AppException(
        AppCts.ProblemDetailsTypes.BadRequest,
        "Identity error",
        string.Join(", ", errors.Select(e => e.Description))
    );
}
