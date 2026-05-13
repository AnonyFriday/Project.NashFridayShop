using FluentValidation;

namespace NashFridayStore.API.Features.Admin.Orders.GetOrderById;

public sealed class Validator : AbstractValidator<Request>
{
    public const string IdIsRequired = "Order ID is required.";

    public Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(IdIsRequired);
    }
}
