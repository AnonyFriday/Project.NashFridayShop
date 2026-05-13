using FluentValidation;

namespace NashFridayStore.API.Features.Admin.Orders.GetOrders;

public sealed class Validator : AbstractValidator<Request>
{
    public const string PageIndexGreaterThanOrEqualZero = "Page index must be greater than or equal to 0.";
    public const string PageSizeGreaterThanOrEqualZero = "Page size must be greater than or equal to 0.";

    public Validator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PageIndexGreaterThanOrEqualZero);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PageSizeGreaterThanOrEqualZero);
    }
}
