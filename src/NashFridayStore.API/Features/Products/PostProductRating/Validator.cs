using FluentValidation;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Products.PostProductRating;

public sealed class Validator : AbstractValidator<Request>
{
    public const string ProductIdIsRequired = "Product Id is required.";
    public const string StarsInRange = "Stars must be between 1 and 10.";
    public const string CommentNotExceedLength = "Comment must not exceed 1000 characters.";

    public Validator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(ProductIdIsRequired);

        RuleFor(x => x.RequestBody.Stars)
            .InclusiveBetween(AppCts.Api.MinStars, AppCts.Api.MaxStars)
            .WithMessage(StarsInRange)
            .OverridePropertyName("Stars");

        RuleFor(x => x.RequestBody.Comment)
            .MaximumLength(1000)
            .WithMessage(CommentNotExceedLength)
            .OverridePropertyName("Comment");
    }
}
