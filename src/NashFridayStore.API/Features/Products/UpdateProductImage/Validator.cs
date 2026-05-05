using FluentValidation;

namespace NashFridayStore.API.Features.Products.UpdateProductImage;

public sealed class Validator : AbstractValidator<Request>
{
    public const string IdRequired = "Product Id is required.";
    public const string ImageRequired = "Product image is required.";

    public Validator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .WithMessage(IdRequired);

        RuleFor(x => x.ImageFile)
            .NotNull()
            .WithMessage(ImageRequired);
    }
}
