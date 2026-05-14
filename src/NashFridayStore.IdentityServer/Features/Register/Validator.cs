using FluentValidation;

namespace NashFridayStore.IdentityServer.Features.Register;

public sealed class Validator : AbstractValidator<Request>
{
    public const string EmailRequired = "Email is required.";
    public const string EmailInvalid = "Email format is not valid.";
    public const string PasswordRequired = "Password is required.";
    public const string PasswordMinLength = "Password must be at least 6 characters.";
    public const string ConfirmPasswordRequired = "Confirm password is required.";
    public const string PasswordMismatch = "Passwords do not match.";
    public const string PhoneInvalid = "Phone number must be 10 digits.";

    public Validator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(EmailRequired)
            .EmailAddress().WithMessage(EmailInvalid);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(PasswordRequired)
            .MinimumLength(6).WithMessage(PasswordMinLength);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(ConfirmPasswordRequired)
            .Equal(x => x.Password).WithMessage(PasswordMismatch);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\d{10}$")
            .WithMessage(PhoneInvalid)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
