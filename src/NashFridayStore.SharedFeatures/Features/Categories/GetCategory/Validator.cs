using FluentValidation;

namespace NashFridayStore.SharedFeatures.Features.Categories.GetCategory;

public sealed class Validator : AbstractValidator<Request>
{
    public const string CategoryIdRequired = "Category Id is required.";

    public Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(CategoryIdRequired);
    }
}


