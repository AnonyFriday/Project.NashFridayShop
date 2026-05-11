using FluentValidation;

namespace NashFridayStore.API.Features.Customer.Orders.CreateCheckout;

public sealed class Validator : AbstractValidator<Request>
{
    public const string CustomerNameRequired = "Customer name is required.";
    public const string CustomerEmailRequired = "Customer email is required.";
    public const string CustomerEmailInvalid = "Customer email is invalid.";
    public const string DeliveryAddressRequired = "Delivery address is required.";
    public const string PhoneNumberRequired = "Phone number is required.";
    public const string CustomerNameMaxLength = "Customer name must not exceed 100 characters.";
    public const string CustomerEmailMaxLength = "Customer email must not exceed 100 characters.";
    public const string DeliveryAddressMaxLength = "Delivery address must not exceed 300 characters.";
    public const string PhoneNumberMaxLength = "Phone number must not exceed 20 characters.";

    public Validator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage(CustomerNameRequired)
            .MaximumLength(100).WithMessage(CustomerNameMaxLength);

        RuleFor(x => x.CustomerEmail)
            .NotEmpty().WithMessage(CustomerEmailRequired)
            .EmailAddress().WithMessage(CustomerEmailInvalid)
            .MaximumLength(100).WithMessage(CustomerEmailMaxLength);

        RuleFor(x => x.DeliveryAddress)
            .NotEmpty().WithMessage(DeliveryAddressRequired)
            .MaximumLength(300).WithMessage(DeliveryAddressMaxLength);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(PhoneNumberRequired)
            .MaximumLength(20).WithMessage(PhoneNumberMaxLength);
    }
}