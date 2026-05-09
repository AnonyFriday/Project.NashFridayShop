using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NashFridayStore.IdentityServer.AppOptions;
using NashFridayStore.IdentityServer.Commons;
using NashFridayStore.IdentityServer.Domain;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace NashFridayStore.IdentityServer.Data;

public class IdentityServerDbContextSeeder(
    IdentityServerDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IOptions<SiteUrlsOption> options,
    IOpenIddictApplicationManager openIddictApplicationManager,
    ILogger<IdentityServerDbContext> logger)
{
    public async Task SeedAccountsAsync()
    {
        logger.LogInformation("Starting accounts seeding...");
        await dbContext.Database.MigrateAsync();

        if (await userManager.Users.AnyAsync()
                && await dbContext.Admins.AnyAsync()
                && await dbContext.Customers.AnyAsync())
        {
            logger.LogInformation("Identity DB already seeded. Skipping.");
            return;
        }

        #region Roles 
        string[] roles = [AppCts.Identity.Roles.Admin, AppCts.Identity.Roles.Customer];
        foreach (string role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
        #endregion

        #region Admins
        var admin = new Admin
        {
            Id = Guid.Parse("1e9a4c73-5d28-46bf-8c91-2f7d6a3b0e66"),
            FullName = "Vu Thi Rau",
            Address = "Random Address which you dont know",
            UserName = "admin@nashstore.com",
            Email = "admin@nashstore.com",
            PhoneNumber = "0123456789",
            AvatarUrl = "https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/1e9a4c73-5d28-46bf-8c91-2f7d6a3b0e66/avataaars.png",
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };

        await userManager.CreateAsync(admin, "Admin@123");
        await userManager.AddToRoleAsync(admin, AppCts.Identity.Roles.Admin);
        await dbContext.SaveChangesAsync();
        #endregion

        #region Customers
        var customerData = new[]
        {
            new { Id = "2c7a9d14-1e5f-4a98-b2e4-3d9f6c0a7e25", Name = "Aria Sterling" },
            new { Id = "3f8c1b7a-4d95-4e2f-a6c1-7b3d9e5f1aaa", Name = "Julian Vane" },
            new { Id = "5d7a2c8f-1b93-4ea6-8fd1-9c2e5b7a1eee", Name = "Elena Rossi" },
            new { Id = "6c3b1a9d-2e84-4f7c-b5a1-9d2e6f3b8c88", Name = "Xavier Thorne" },
            new { Id = "7b6e3d92-8a11-4f5b-90cd-6e3f1a7b2d44", Name = "Seraphina Moon" },
            new { Id = "8a1e5d3c-7f42-4b9a-b3d8-6f1c2e7a9ccc", Name = "Silas Vance" },
            new { Id = "9f1b7f5d-4d83-4c7e-a3b1-8b8f2f7f6a11", Name = "Lyra Belacqua" },
            new { Id = "a5f2d9c8-7b31-4e6a-93fd-8c1b5e2d7f77", Name = "Cyrus Drake" },
            new { Id = "b2d7f4a9-6c18-45eb-91fa-0e7c3b2d8bbb", Name = "Nova Skye" },
            new { Id = "c8d1f6a5-3b7e-4d99-a1c2-5f8e7b6d9a55", Name = "Orion Pax" },
            new { Id = "d7e5a2c1-9b64-41fa-8e3d-4c7b1a9f0d99", Name = "Isolde Thorne" },
            new { Id = "e4c9b1d7-2a56-4f8e-95bc-3a7d1f6e0ddd", Name = "Kaelen Voss" },
            new { Id = "f4d2b8a1-6c93-47ea-9f7c-1a2d5e8b4c30", Name = "Thalia Muse" }
        };

        int customerIndex = 1;
        foreach (var data in customerData)
        {
            var customer = new Customer
            {
                Id = Guid.Parse(data.Id),
                FullName = data.Name,
                Address = "Random Address " + customerIndex,
                UserName = $"customer{customerIndex}@nashstore.com",
                Email = $"customer{customerIndex}@nashstore.com",
                PhoneNumber = "0987654321",
                AvatarUrl = $"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{data.Id}/avataaars.png",
                CreatedAtUtc = DateTime.UtcNow,
                IsDeleted = false
            };

            await userManager.CreateAsync(customer, $"Customer{customerIndex}@123");
            await userManager.AddToRoleAsync(customer, AppCts.Identity.Roles.Customer);
            customerIndex++;
        }
        await dbContext.SaveChangesAsync();
        #endregion

        logger.LogInformation("Identity database seeding completed.");
    }

    public async Task SeedBff()
    {
        logger.LogInformation("Starting BFF seeding...");
        BffOptions bffOptions = options.Value.Bff;

        object? existingClient = await openIddictApplicationManager.FindByClientIdAsync(bffOptions.ClientId);

        if (existingClient != null)
        {
            logger.LogInformation("BFF client alreay exists.");
            return;
        }

        await openIddictApplicationManager.CreateAsync(new OpenIddictApplicationDescriptor()
        {
            ClientId = bffOptions.ClientId,
            ClientSecret = bffOptions.ClientSecret,
            RedirectUris =
            {
                new Uri($"{bffOptions.SignInCallbackUrl}")
            },

            PostLogoutRedirectUris =
            {
                new Uri($"{bffOptions.SignOutCallbackUrl}")
            },

            Permissions =
            {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.EndSession,

                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,

                    Permissions.ResponseTypes.Code,

                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + bffOptions.ApiScope
            },

            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        });

        logger.LogInformation("BFF client seeded successfully.");
    }

    public async Task SeedOidcDebugger()
    {
        logger.LogInformation("Starting OIDC Debugger seeding...");
        OidcDebuggerOptions oidcDebuggerOptions = options.Value.OidcDebugger;

        object? existingClient = await openIddictApplicationManager.FindByClientIdAsync(oidcDebuggerOptions.ClientId);

        if (existingClient != null)
        {
            logger.LogInformation("BFF client alreay exists.");
            return;
        }

        await openIddictApplicationManager.CreateAsync(new OpenIddictApplicationDescriptor()
        {
            ClientId = oidcDebuggerOptions.ClientId,
            ClientType = ClientTypes.Public,

            RedirectUris =
            {
                new Uri($"{oidcDebuggerOptions.SignInCallbackUrl}")
            },

            Permissions =
            {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.EndSession,

                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,

                    Permissions.ResponseTypes.Code,

                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + oidcDebuggerOptions.ApiScope
            },

            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        });

        logger.LogInformation("Oidc Debugger client seeded successfully.");
    }
}
