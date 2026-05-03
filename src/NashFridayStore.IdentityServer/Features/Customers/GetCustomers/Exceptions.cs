using FluentValidation.Results;
using NashFridayStore.IdentityServer.Commons.Exceptions;

namespace NashFridayStore.IdentityServer.Features.Customers.GetCustomers;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);
}
