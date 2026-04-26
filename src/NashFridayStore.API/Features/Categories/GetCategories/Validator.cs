using FluentValidation;

namespace NashFridayStore.API.Features.Categories.GetCategories;

public sealed class Validator : AbstractValidator<Request>
{
    public const string SearchNameMaxLength = "Search has a maximum length of 100 characters.";
    public const string PageIndexGreaterThanOrEqualZero = "Page must be greater than or equal to 0.";
    public const string PageSizeBetweenRange = "Page Size must be between 1 and 100.";

    public Validator()
    {
        RuleFor(x => x.SearchName)
            .MaximumLength(100)
            .WithMessage(SearchNameMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchName));

        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PageIndexGreaterThanOrEqualZero)
            .When(x => !x.IsAll);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(PageSizeBetweenRange)
            .When(x => !x.IsAll);
    }
}
