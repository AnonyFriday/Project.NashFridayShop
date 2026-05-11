using NashFridayStore.API.Commons.Exceptions;
using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.API.Features.Customer.Orders.HandleStripeWebhook;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> errors) : AppValidationException(errors);

    public sealed class InvalidSignatureException() : AppException(
        ProblemDetailsTypes.BadRequest,
        "Invalid Stripe Signature",
        "The Stripe-Signature header is invalid or could not be verified."
    );
}
