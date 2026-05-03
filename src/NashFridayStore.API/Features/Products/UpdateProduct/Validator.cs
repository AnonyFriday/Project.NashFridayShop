using FluentValidation;

namespace NashFridayStore.API.Features.Products.UpdateProduct;

public sealed class Validator : AbstractValidator<Request>
{
    public const string IdRequired = "Product Id is required.";
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
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(IdRequired);

        RuleFor(x => x.RequestBody.CategoryId)
            .NotEmpty()
            .OverridePropertyName("CategoryId")
            .WithMessage(CategoryIdRequired);


        RuleFor(x => x.RequestBody.Name)
            .NotEmpty()
            .OverridePropertyName("Name")
            .WithMessage(NameRequired)
            .MaximumLength(100)
            .WithMessage(NameMaxLength);

        RuleFor(x => x.RequestBody.Description)
            .NotEmpty()
            .OverridePropertyName("Description")
            .WithMessage(DescriptionRequired)
            .MaximumLength(300)
            .WithMessage(DescriptionMaxLength);

        RuleFor(x => x.RequestBody.PriceUsd)
            .GreaterThan(0)
            .WithMessage(PriceGreaterThanZero);

        RuleFor(x => x.RequestBody.ImageUrl)
            .NotEmpty()
            .WithMessage(ImageUrlRequired);

        RuleFor(x => x.RequestBody.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage(QuantityGreaterThanOrEqualZero);

        RuleFor(x => x.RequestBody.Status)
            .IsInEnum()
            .WithMessage(InvalidProductStatus);
    }
}
