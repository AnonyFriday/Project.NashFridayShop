using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Categories.GetCategories;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
    {
        // Cleaning Request
        int pageIndex;
        if (orgReq.IsAll)
        {
            pageIndex = 0;
        }
        else
        {
            pageIndex = orgReq.PageIndex <= 0 ? AppCts.Api.PageIndex : orgReq.PageIndex;
        }

        int pageSize;
        if (orgReq.IsAll)
        {
            pageSize = int.MaxValue;
        }
        else
        {
            pageSize = orgReq.PageSize < 0 ? AppCts.Api.PageSize : orgReq.PageSize;
        }

        Request req = orgReq with
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchName = string.IsNullOrWhiteSpace(orgReq.SearchName) ? null : orgReq.SearchName.Trim()
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Implementing logic
        IQueryable<Category> query = dbContext.Categories.AsQueryable();

        query = query
            .AsNoTracking()
            .Where(x =>
                string.IsNullOrWhiteSpace(req.SearchName) || x.Name.Contains(req.SearchName)
            );

        int totalItems = await query.CountAsync(ct);

        List<CategoryItem> items = await query
            .OrderByDescending(x => x.Name)
            .Skip(req.PageIndex * req.PageSize)
            .Take(req.PageSize)
            .Select(x => new CategoryItem(
                x.Id,
                x.Name,
                x.Description))
            .ToListAsync(ct);

        int totalPages = orgReq.IsAll ? 1 : (int)Math.Ceiling(totalItems / (double)req.PageSize);

        return new Response(
            items,
            totalItems,
            totalPages,
            req.PageIndex);
    }
}
