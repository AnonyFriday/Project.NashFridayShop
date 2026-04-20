using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Infrastructure.Data;


namespace NashFridayStore.API.Features.Categories.GetCategory;

#region Contracts
public sealed record Request(Guid Id);

public sealed record Response(
    Guid Id,
    string Name,
    string Description);
#endregion

#region Validation
public sealed class Validator : AbstractValidator<Request>
{
    public const string IdRequired = "Category Id is required.";

    public Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(IdRequired);
    }
}
#endregion

#region Business Logic
public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
    {
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw GetCategoryErrors.Validation(validation.Errors);
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
            throw GetCategoryErrors.NotFound(req.Id);
        }

        return category;
    }
}
#endregion

#region Endpoints
[ApiController]
[Route("api/categories/{id:guid}")]
public class GetCategoryController(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new Request(id);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
#endregion
