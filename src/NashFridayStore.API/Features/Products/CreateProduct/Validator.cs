using FluentValidation;

namespace NashFridayStore.API.Features.Products.CreateProduct;

public sealed class Validator : AbstractValidator<Request>
{
    public const string CategoryIdRequired = "Category Id is required.";
    public const string NameRequired = "Product name is required.";
    public const string NameMaxLength = "Product name must not exceed 100 characters.";
    public const string DescriptionRequired = "Product description is required.";
    public const string DescriptionMaxLength = "Product description must not exceed 300 characters.";
    public const string PriceGreaterThanZero = "Price must be greater than 0.";
    public const string ImageUrlRequired = "Image URL is required.";
    public const string QuantityGreaterThanOrEqualZero = "Quantity must be greater than or equal to 0.";
    public const string InvalidProductStatus = "Invalid product status.";

    public Validator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(CategoryIdRequired);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(NameRequired)
            .MaximumLength(100)
            .WithMessage(NameMaxLength);

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage(ImageUrlRequired);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(DescriptionRequired)
            .MaximumLength(300)
            .WithMessage(DescriptionMaxLength);

        RuleFor(x => x.PriceUsd)
            .GreaterThan(0)
            .WithMessage(PriceGreaterThanZero);

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage(QuantityGreaterThanOrEqualZero);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(InvalidProductStatus);
    }
}
