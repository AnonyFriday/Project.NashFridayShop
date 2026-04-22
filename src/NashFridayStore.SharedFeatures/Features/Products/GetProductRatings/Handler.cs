using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.SharedFeatures.Features.Products.GetProductRatings;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> Handle(Request orgReq, CancellationToken ct)
    {
        // Cleaning Request
        Request req = orgReq with
        {
            PageIndex = orgReq.PageIndex <= 0 ? AppCts.Api.PageIndex : orgReq.PageIndex,
            PageSize = orgReq.PageSize < 0 ? AppCts.Api.PageSize : orgReq.PageSize,
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Check if product exists
        IQueryable<Product> queryProduct = dbContext.Products.AsQueryable();

        if (req.IncludeDeleted)
        {
            queryProduct = queryProduct.IgnoreQueryFilters();
        }

        bool productExists = await queryProduct
            .AsNoTracking()
            .AnyAsync(x => x.Id == req.ProductId, ct);

        if (!productExists)
        {
            throw new Exceptions.ProductNotFoundException(req.ProductId);
        }

        // Get ratings
        IQueryable<ProductRating> queryRating = dbContext.ProductRatings.AsQueryable();

        if (req.IncludeDeleted)
        {
            queryRating = queryRating.IgnoreQueryFilters();
        }

        queryRating = queryRating
            .AsNoTracking()
            .Where(x =>
                x.ProductId == req.ProductId);

        int totalItems = await queryRating.CountAsync(ct);

        List<RatingItem> items = await queryRating
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip(req.PageIndex * req.PageSize)
            .Take(req.PageSize)
            .Select(x => new RatingItem(
                x.Stars,
                x.Comment,
                x.CreatedAtUtc))
            .ToListAsync(ct);

        // Calculate average
        decimal average = await queryRating
            .Select(x => (decimal?)x.Stars)
            .AverageAsync(ct) ?? 0;

        int totalPages = (int)Math.Ceiling(totalItems / (double)req.PageSize);

        return new Response(
            items,
            totalItems,
            totalPages,
            req.PageIndex,
            average);
    }
}
