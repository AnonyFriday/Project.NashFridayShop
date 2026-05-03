using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Products.GetProduct;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
    {
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        IQueryable<Product> query = dbContext.Products;

        if (req.IncludeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        Response product = await query
            .AsNoTracking()
            .Include(p => p.ProductRatings)
            .Where(x => x.Id == req.Id)
            .Select(x => new Response(
                x.Id,
                x.CategoryId,
                x.Name,
                x.Description,
                x.ImageUrl,
                x.PriceUsd,
                x.Quantity,
                x.Status,
                x.ProductRatings.Any() ? x.ProductRatings.Average(r => (decimal)r.Stars) % AppCts.Api.MaxStars : 0))
            .FirstOrDefaultAsync(ct);

        if (product is null)
        {
            throw new Exceptions.ProductNotFoundException(req.Id);
        }

        string? categoryName = await dbContext.Categories
                    .Where(x => x.Id.Equals(product.CategoryId))
                    .Select(x => x.Name)
                    .FirstOrDefaultAsync(ct);

        product.CategoryName = categoryName!;

        return product;
    }
}
