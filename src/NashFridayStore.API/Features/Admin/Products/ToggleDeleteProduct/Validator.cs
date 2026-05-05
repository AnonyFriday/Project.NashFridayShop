using FluentValidation;

namespace NashFridayStore.API.Features.Admin.Products.ToggleDeleteProduct;

public sealed class Validator : AbstractValidator<Request>
{
    public const string IdRequired = "Product Id is required.";

    public Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(IdRequired);
    }
}
