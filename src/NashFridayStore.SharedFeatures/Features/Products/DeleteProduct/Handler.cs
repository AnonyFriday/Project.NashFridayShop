using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.SharedFeatures.Features.Products.DeleteProduct;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
    {
        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Get product
        Product? product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: ct);

        if (product == null)
        {
            throw new Exceptions.ProductNotFoundException(req.Id);
        }

        // Perform soft delete
        product.IsDeleted = true;
        product.DeletedAtUtc = DateTime.UtcNow;

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(ct);

        return new Response(
            product.Id,
            product.IsDeleted,
            product.DeletedAtUtc ?? DateTime.UtcNow);
    }
}
