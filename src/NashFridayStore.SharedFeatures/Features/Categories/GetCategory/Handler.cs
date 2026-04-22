using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.API.Features.Categories.GetCategory;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.SharedFeatures.Features.Categories.GetCategory;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
    {
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        Response? category = await dbContext.Categories
            .AsNoTracking()
            .Where(x => x.Id == req.Id)
            .Select(x => new Response(
                x.Id,
                x.Name,
                x.Description))
            .FirstOrDefaultAsync(ct);

        if (category is null)
        {
            throw new Exceptions.CategoryNotFoundException(req.Id);
        }

        return category;
    }
}


