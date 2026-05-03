using FluentValidation;

namespace NashFridayStore.API.Features.Products.GetProducts;

public sealed class Validator : AbstractValidator<Request>
{
    public const string SearchNameMaxLength = "Search has a maximum length of 100 characters.";
    public const string MinPriceLessThanOrEqualMaxPrice = "Min Price must be less than or equal to Max Price.";
    public const string PageIndexGreaterThanOrEqualZero = "Page must be greater than or equal to 0.";
    public const string PageSizeBetweenRange = "Page Size must be between 1 and 100.";
    public const string InvalidProductStatus = "Invalid product status.";
    public const string MinPriceGreaterThanOrEqualZero = "Min Price must be greater than or equal to 0.";
    public const string MaxPriceGreaterThanOrEqualZero = "Max Price must be greater than or equal to 0.";

    public Validator()
    {
        RuleFor(x => x.SearchName)
            .MaximumLength(100)
            .WithMessage(SearchNameMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchName));

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MinPriceGreaterThanOrEqualZero)
            .When(x => x.MinPrice.HasValue);

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MaxPriceGreaterThanOrEqualZero)
            .When(x => x.MaxPrice.HasValue);

        RuleFor(x => x)
            .Must(x => x.MinPrice <= x.MaxPrice)
            .WithMessage(MinPriceLessThanOrEqualMaxPrice)
            .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);

        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PageIndexGreaterThanOrEqualZero);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(PageSizeBetweenRange);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(InvalidProductStatus)
            .When(x => x.Status.HasValue);
    }
}
