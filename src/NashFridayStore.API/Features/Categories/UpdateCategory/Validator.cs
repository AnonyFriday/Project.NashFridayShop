using FluentValidation;

namespace NashFridayStore.API.Features.Categories.UpdateCategory;

public sealed class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Body.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Body.Description)
            .NotEmpty()
            .MaximumLength(300);
    }
}
