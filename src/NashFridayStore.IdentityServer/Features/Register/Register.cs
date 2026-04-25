using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using NashFridayStore.IdentityServer.Domain;

namespace NashFridayStore.IdentityServer.Features.Register;

public record Request
{
    public string Email { get; set; }

    public string FullName { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string? PhoneNumber { get; set; }
}

public record Response
{
    public string Message { get; set; }
}

public class Valiator : AbstractValidator<Request>
{
    public const string EmailRequired = "Email is required.";
    public const string EmailInvalid = "Email format is not valid";

    public const string PasswordRequired = "Password is required";
    public const string PasswordMinLength = "Password must have a length at least 6 characters";

    public const string ConfirmPasswordRequired = "Confirm password is required";
    public const string PasswordMismatch = "Passwords does not match.";

    public const string PhoneInvalid = "Phone Number must be in 10 digits";

    public Valiator()
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
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage(PhoneInvalid);
    }
}



#pragma warning disable S1481 // Unused local variables should be removed
#pragma warning disable CS9113 // Parameter is unread.
public class Handler(UserManager<ApplicationUser> userManager, IValidator<Request> validator)
#pragma warning restore CS9113 // Parameter is unread.
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
    {
        // Clean Request
        Request req = orgReq with
        {
            ConfirmPassword = orgReq.ConfirmPassword.Trim(),
            Email = orgReq.Email.Trim(),
            FullName = orgReq.FullName.Trim(),
            Password = orgReq.ConfirmPassword.Trim(),
            PhoneNumber = orgReq.ConfirmPassword.Trim()
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);

        return new Response();
    }
}

#pragma warning restore S1481 // Unused local variables should be removed
