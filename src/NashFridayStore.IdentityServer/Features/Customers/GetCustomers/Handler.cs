using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.IdentityServer.Commons;
using NashFridayStore.IdentityServer.Data;
using NashFridayStore.IdentityServer.Domain;

namespace NashFridayStore.IdentityServer.Features.Customers.GetCustomers;

public sealed class Handler(
    IdentityServerDbContext dbContext,
    IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
    {
        // Pre clean request
        Request req = orgReq with
        {
            PageIndex = orgReq.PageIndex <= 0 ? AppCts.Api.PageIndex : orgReq.PageIndex,
            PageSize = orgReq.PageSize < 0 ? AppCts.Api.PageSize : orgReq.PageSize,
            SearchName = !string.IsNullOrWhiteSpace(orgReq.SearchName) ? orgReq.SearchName.Trim() : null
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        IQueryable<ApplicationUser> query = dbContext.Users.OfType<Customer>();

        if (req.IncludeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        query = query
            .AsNoTracking()
            .Where(x =>
                string.IsNullOrWhiteSpace(req.SearchName) ||
                (x.FullName != null && x.FullName.ToLower().Contains(req.SearchName.ToLower())) ||
                (x.Email != null && x.Email.ToLower().Contains(req.SearchName.ToLower()))
            );

        int totalItems = await query.CountAsync(ct);

        List<CustomerItem> items = await query
            .OrderByDescending(u => u.CreatedAtUtc)
            .Skip(req.PageIndex * req.PageSize)
            .Take(req.PageSize)
            .Select(u => new CustomerItem(
                u.Id,
                u.FullName,
                u.UserName!,
                u.Email!,
                u.Address,
                u.IsDeleted,
                u.CreatedAtUtc))
            .ToListAsync(ct);

        int totalPages = (int)Math.Ceiling(totalItems / (double)req.PageSize);

        return new Response(
            items,
            totalItems,
            totalPages,
            req.PageIndex);
    }
}
