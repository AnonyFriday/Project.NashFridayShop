using FluentValidation;

namespace NashFridayStore.API.Features.Customer.Cart.CreateOrAddItemToCart;

public sealed class Validator : AbstractValidator<Request>
{
    public const string ProductIdRequired = "Product ID is required.";
    public const string QuantityInvalid = "Quantity must not be zero.";
    public const string ProductNameRequired = "Product name is required.";
    public const string PriceInvalid = "Price must be greater than zero.";
    public const string CategoryIdRequired = "Category ID is required.";
    public const string CategoryNameRequired = "Category name is required.";

    public Validator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage(ProductIdRequired);
        RuleFor(x => x.Quantity).Must(x => x != 0).WithMessage(QuantityInvalid);
        RuleFor(x => x.ProductName).NotEmpty().WithMessage(ProductNameRequired);
        RuleFor(x => x.Price).GreaterThan(0).WithMessage(PriceInvalid);
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage(CategoryIdRequired);
        RuleFor(x => x.CategoryName).NotEmpty().WithMessage(CategoryNameRequired);
    }
}
