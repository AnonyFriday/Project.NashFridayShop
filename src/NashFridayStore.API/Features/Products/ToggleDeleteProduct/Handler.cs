using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Products.ToggleDeleteProduct;

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

        Product? product = await dbContext.Products
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: ct);

        if (product == null)
        {
            throw new Exceptions.ProductNotFoundException(req.Id);
        }

        if (product.IsDeleted)
        {
            product.IsDeleted = false;
        }
        else
        {
            product.IsDeleted = true;
            product.DeletedAtUtc = DateTime.UtcNow;
        }

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(ct);

        return new Response(
            product.Id,
            product.IsDeleted,
            product.DeletedAtUtc ?? DateTime.UtcNow);
    }
}
