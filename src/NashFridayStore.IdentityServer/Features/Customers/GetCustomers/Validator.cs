using FluentValidation;

namespace NashFridayStore.IdentityServer.Features.Customers.GetCustomers;

public sealed class Validator : AbstractValidator<Request>
{
    public const string PageIndexGreaterThanOrEqualZero = "Page must be greater than or equal to 0.";
    public const string PageSizeBetweenRange = "Page Size must be between 1 and 100.";

    public Validator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PageIndexGreaterThanOrEqualZero);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(PageSizeBetweenRange);
    }
}
