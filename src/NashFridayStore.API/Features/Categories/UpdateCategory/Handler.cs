using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Categories.UpdateCategory;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
    {
        // Clean request
        Request req = orgReq with
        {
            Body = orgReq.Body with
            {
                Name = orgReq.Body.Name.Trim(),
                Description = orgReq.Body.Description.Trim(),
            }
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Get category
        Category? category = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: ct);

        if (category == null)
        {
            throw new Exceptions.CategoryNotFoundException(req.Id);
        }

        // Check for duplicate name (excluding itself)
        bool nameExists = await dbContext.Categories
            .AnyAsync(x => x.Name == req.Body.Name && x.Id != req.Id, ct);

        if (nameExists)
        {
            throw new Exceptions.CategoryAlreadyExistsException(req.Body.Name);
        }

        category.Name = req.Body.Name;
        category.Description = req.Body.Description;

        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync(ct);

        return new Response(
            category.Id,
            category.Name,
            category.Description);
    }
}
