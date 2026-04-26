using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Products.PostProductRating;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
    {
        // Cleaning Request
        Request req = orgReq with
        {
            RequestBody = orgReq.RequestBody with
            {
                Comment = string.IsNullOrWhiteSpace(orgReq.RequestBody.Comment)
                    ? null
                    : orgReq.RequestBody.Comment.Trim(),
            }
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Business Logic
        Product? product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.Id == req.ProductId, cancellationToken: ct);

        if (product == null)
        {
            throw new Exceptions.ProductNotFoundException(req.ProductId);
        }

        var newRating = new ProductRating
        {
            Id = Guid.NewGuid(),
            ProductId = req.ProductId,
            Stars = req.RequestBody.Stars,
            Comment = req.RequestBody.Comment,
            CreatedAtUtc = DateTime.UtcNow
        };

        dbContext.ProductRatings.Add(newRating);
        await dbContext.SaveChangesAsync(ct);

        return new Response(
            newRating.ProductId,
            newRating.Stars,
            newRating.Comment,
            newRating.CreatedAtUtc);
    }
}
