using FluentValidation;

namespace NashFridayStore.API.Features.Customer.Cart.AddCart;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
