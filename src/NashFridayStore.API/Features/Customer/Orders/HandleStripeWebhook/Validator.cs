using FluentValidation;

namespace NashFridayStore.API.Features.Customer.Orders.HandleStripeWebhook;

public sealed class Validator : AbstractValidator<Request>
{
    public const string JsonPayloadRequired = "JSON payload is required.";
    public const string StripeSignatureRequired = "Stripe-Signature header is missing.";

    public Validator()
    {
        RuleFor(x => x.JsonPayload)
            .NotEmpty().WithMessage(JsonPayloadRequired);

        RuleFor(x => x.StripeSignature)
            .NotEmpty().WithMessage(StripeSignatureRequired);
    }
}
