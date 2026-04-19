using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.API.Exceptions;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Categories.GetCategories;

#region Contracts
public sealed record CategoryItem(Guid Id, string Name, string Description);

public sealed record Request(
    string? SearchName,
    int PageIndex = 0,
    int PageSize = 10,
    bool IsAll = false);

public sealed record Response(
    IReadOnlyList<CategoryItem> CategoryItems,
    int TotalItems,
    int TotalPages,
    int PageIndex);
#endregion

#region Validation
public sealed class Validator : AbstractValidator<Request>
{
    public const string SearchNameMaxLength = "Search has a maximum length of 100 characters.";
    public const string PageIndexGreaterThanOrEqualZero = "Page must be greater than or equal to 0.";
    public const string PageSizeBetweenRange = "Page Size must be between 1 and 100.";

    public Validator()
    {
        RuleFor(x => x.SearchName)
            .MaximumLength(100)
            .WithMessage(SearchNameMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchName));

        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PageIndexGreaterThanOrEqualZero)
            .When(x => !x.IsAll);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(PageSizeBetweenRange)
            .When(x => !x.IsAll);
    }
}
#endregion

#region Errors
public static class GetCategoriesErrors
{
    public static RequestValidationException Validation(IList<ValidationFailure> errors)
    {
        return new RequestValidationException(
            errors.Select(e => new RequestValidationError(e.PropertyName, e.ErrorMessage)));
    }
}
#endregion

#region Business Logic
public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> Handle(Request orgReq, CancellationToken ct)
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
            throw GetCategoriesErrors.Validation(validation.Errors);
        }

        // Implementing logic
        IQueryable<Category> query = dbContext.Categories.AsQueryable();

        query = query
            .AsNoTracking()
            .Where(x =>
                (string.IsNullOrWhiteSpace(req.SearchName) || x.Name.Contains(req.SearchName))
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
#endregion

#region Endpoints
[ApiController]
[Route("api/categories")]
public class GetCategoriesController(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Request request,
        CancellationToken ct
    )
    {
        Response response = await handler.Handle(request, ct);
        return Ok(response);
    }
}
#endregion
