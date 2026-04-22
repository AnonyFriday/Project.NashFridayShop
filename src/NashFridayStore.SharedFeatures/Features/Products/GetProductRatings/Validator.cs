using FluentValidation;

namespace NashFridayStore.SharedFeatures.Features.Products.GetProductRatings;

public sealed class Validator : AbstractValidator<Request>
{
    public const string PageIndexGreaterThanOrEqualZero = "Page must be greater than or equal to 0.";
    public const string PageSizeBetweenRange = "Page Size must be between 1 and 100.";

    public Validator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product Id is required.");

        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PageIndexGreaterThanOrEqualZero);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(PageSizeBetweenRange);
    }
}
