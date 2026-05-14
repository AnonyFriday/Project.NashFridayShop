using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using NashFridayStore.IdentityServer.Commons;
using NashFridayStore.IdentityServer.Domain;

namespace NashFridayStore.IdentityServer.Features.Register;

public sealed class Handler(UserManager<ApplicationUser> userManager, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
    {
        // Cleaning Request
        Request req = orgReq with
        {
            ConfirmPassword = orgReq.ConfirmPassword.Trim(),
            Email = orgReq.Email.Trim(),
            FullName = orgReq.FullName.Trim(),
            Password = orgReq.Password.Trim(),
            PhoneNumber = orgReq.PhoneNumber?.Trim()
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Create User
        var user = new Customer
        {
            UserName = req.Email,
            Email = req.Email,
            FullName = req.FullName,
            PhoneNumber = req.PhoneNumber,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };

        IdentityResult result = await userManager.CreateAsync(user, req.Password);
        if (!result.Succeeded)
        {
            throw new Exceptions.IdentityException(result.Errors);
        }

        await userManager.AddToRoleAsync(user, AppCts.Identity.Roles.Customer);

        return new Response { Message = "Registration successful." };
    }
}
