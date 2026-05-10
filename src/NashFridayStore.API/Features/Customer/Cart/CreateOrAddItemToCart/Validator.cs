using FluentValidation;

namespace NashFridayStore.API.Features.Customer.Cart.CreateOrAddItemToCart;

public sealed class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).Must(x => x != 0);
        RuleFor(x => x.ProductName).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
