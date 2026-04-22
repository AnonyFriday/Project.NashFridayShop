using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.SharedFeatures.Features.Products.GetProduct;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
    {
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        Response? product = await dbContext.Products
            .AsNoTracking()
            .Include(p => p.ProductRatings)
            .Where(x => x.Id == req.Id)
            .Select(x => new Response(
                x.Id,
                x.Name,
                x.ImageUrl,
                x.PriceUsd,
                x.Status,
                x.ProductRatings.Any() ? x.ProductRatings.Average(r => (decimal)r.Stars) % AppCts.Api.MaxStars : 0))
            .FirstOrDefaultAsync(ct);

        if (product is null)
        {
            throw new Exceptions.ProductNotFoundException(req.Id);
        }

        return product;
    }
}
