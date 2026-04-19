# Nash Friday Shop

![codecov](https://codecov.io/gh/AnonyFriday/Project.NashFridayShop/graph/badge.svg?token=GU9DBWMKZZ)

## Techstack

- **🔧 Backend**: .NET 10 (ASP.NET Core, Entity Framework Core, FluentValidation, JWT Bearer, Scalar)
- **🌐 Frontend**: Next.js (Admin Site), ASP.NET Razor Pages (StoreFront)
- **🧩 BFF**: Backend for Frontend service
- **🔐 Identity**: IdentityServer4
- **🗄️ Database**: SQL Server (via EF Core)
- **🏗️ Architecture**: Vertical Slice Architecture
- **🧪 Testing**: xUnit, Integration Testing, Coverlet
- **🛠️ Tooling**: `Directory.Packages.props`, `Directory.Build.props`, `.editorconfig`

## Current Supporting APIs

| API Endpoint                | Method              | Description                             | Status             |
| --------------------------- | ------------------- | --------------------------------------- | ------------------ |
| `/api/categories`           | GET                 | Category menu / list categories         | ✅ Completed       |
| `/api/products`             | GET                 | Product listing, filters, category view | ✅ Completed       |
| `/api/products/{id}`        | GET                 | Product details                         | ✅ Completed       |
| `/api/products/{id}/rating` | POST                | Product rating                          | ❌ Not implemented |
| `/api/auth/register`        | POST                | Customer registration                   | ❌ Not implemented |
| `/api/auth/login`           | POST                | Customer login                          | ❌ Not implemented |
| `/api/auth/logout`          | POST                | Customer logout                         | ❌ Not implemented |
| `/api/cart`                 | POST/GET            | Shopping cart                           | ❌ Not implemented |
| `/api/order`                | POST                | Order creation                          | ❌ Not implemented |
| `/api/admin/categories`     | GET/POST/PUT/DELETE | Manage categories                       | ❌ Not implemented |
| `/api/admin/products`       | GET/POST/PUT/DELETE | Manage products                         | ❌ Not implemented |

## ERD (V1)

![ERD Diagram](./images/erd_v1_no_identity.png)

## Week 1 Summary

### Setup FE + BE Source Code

- Dev environment: fork, Postman, secrets, `appsettings.Development.json`, `IOptions`
- Coding convention: `.editorconfig`
- Central package management: `Directory.Packages.props`
- Global analyzer: `Directory.Build.props`
- SonarAnalyzer
- DbContext setup
- Razor Pages frontend
- BFF service scaffold
- Identity Server scaffold
- Next.js admin site scaffold
- CORS configuration
- Code coverage setup
- HTTPS support: `mkcert`, `dotnet dev-certs`, Next.js dev cert
- Documentation tool: Scalar
- API versioning
- Testing stack: xUnit, SQLite, `WebApplicationFactory`
- Exception handling: API exception, validation exception, general exception handler

### CI/CD and Services

- Next.JS CI setup
- .NET Projects CI setup
- CD (later)
- Docker Compose support: Redis, SQL Server, Redis Insight

### Research and Architecture

- Identity service research
- BFF research
- Vertical Slice Architecture research

### Feature Foundation

- Entity and configuration setup for Product and Category
- Migrations and seed data preparation

## Project Structure

```
src/
├── ApiService/
│   ├── NashFridayStore.API/          # Main API service
│   ├── NashFridayStore.Domain/       # Domain entities and business rules
│   ├── NashFridayStore.Infrastructure/ # Data access and external services
│   └── NashFridayStore.Tests/         # Unit and integration tests
├── BFF/                              # Backend for Frontend
├── IdentityServer/                   # Authentication service
├── StoreFront/                       # Customer-site ASP.NET Razor Pages app
└── admin-site/                       # Admin Next.js application
```

## Vertical Slice Architecture Explanation

Each feature is organized in its own "slice" with all related code together:

- **Contracts**: Request/Response records
- **Validation**: FluentValidation rules
- **Business Logic**: Handler with domain logic
- **Endpoints**: Controller actions
- **Errors**: Custom exception handling
- **Tests**: Unit and integration tests per feature

Example from `src/ApiService/NashFridayStore.API/Features/Products/GetProduct/GetProduct.cs`:

```csharp
// Request contract for the GetProduct feature
public sealed record Request(Guid Id);

// Response contract returned by the handler
public sealed record Response(Guid Id, string Name, string ImageUrl, decimal PriceUsd, ProductStatus Status);

public sealed class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product Id is required.");
    }
}

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> Handle(Request req, CancellationToken ct)
    {
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw GetProductErrors.Validation(validation.Errors);
        }

        Response? product = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.Id == req.Id)
            .Select(x => new Response(x.Id, x.Name, x.ImageUrl, x.PriceUsd, x.Status))
            .FirstOrDefaultAsync(ct);

        if (product is null)
        {
            throw GetProductErrors.NotFound(req.Id);
        }

        return product;
    }
}

[ApiController]
[Route("api/products/{id:guid}")]
public class GetProductController(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new Request(id);
        Response response = await handler.Handle(request, ct);
        return Ok(response);
    }
}
```

Error factory example from `src/ApiService/NashFridayStore.API/Features/Products/GetProduct/GetProductErrors.cs`:

```csharp
internal static class GetProductErrors
{
    internal static RequestValidationException Validation(IList<ValidationFailure> errors)
    {
        return new RequestValidationException(
            errors.Select(e => new RequestValidationError(e.PropertyName, e.ErrorMessage)));
    }

    internal static ApiResponseException NotFound(Guid id)
    {
        return new ApiResponseException(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Product not found.",
            Detail = $"Product with id '{id}' was not found.",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"
        });
    }
}
```

Unit test example:

```csharp
[Fact]
public void Validate_IdIsEmpty_ShouldHaveValidationError()
{
    var request = new Request(Guid.Empty);
    TestValidationResult<Request> result = _validator.TestValidate(request);
    Assert.False(result.IsValid);
}
```

Application test example:

```csharp
HttpResponseMessage response = await _client.GetAsync($"/api/products/{product.Id}", cancellationToken);
response.EnsureSuccessStatusCode();
Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);
```

This keeps features isolated and easy to maintain.

## Design Patterns Applied

- **Options Pattern**: config via `IOptions<T>`
- **Dependency Injection**: register services and handlers
- **Builder Pattern**: test and seed helpers
- **Vertical Slice Architecture**: feature-based structure

## Reference Links

### Frontend Stack

- CI example: https://santhosh-adiga-u.medium.com/setting-up-a-complete-ci-cd-pipeline-for-react-using-github-actions-9a07613ceded

### Backend

- SQL Server Docker image: https://hub.docker.com/r/microsoft/mssql-server
- DbContext pooling guidance: https://medium.com/@razeshmsb02/adddbcontext-vs-adddbcontextpool-vs-adddbcontextfactory-3760857737d1
- FluentValidation DI docs: https://docs.fluentvalidation.net/en/latest/di.html
- Modern exception handling: https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8
- ProblemDetails / API error handling: https://medium.com/@aseem2372005/handling-api-errors-the-right-way-understanding-problemdetails-in-asp-net-core-web-api-e3f7d404672c
- HTTP error type reference: https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1
- Content type RFC: https://datatracker.ietf.org/doc/html/rfc7807

### Architecture & Testing

- Vertical Slice Architecture guide: https://nadirbad.dev/vertical-slice-architecture-dotnet
- Integration testing in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=xunit
- SQLite in-memory databases: https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/in-memory-databases
- Avoid in-memory DBs for tests: https://www.jimmybogard.com/avoid-in-memory-databases-for-tests/#:~:text=What%20are%20you%20using%20to,fail%20on%20the%20real%20thing.
- Test execution serially: https://stackoverflow.com/questions/1408175/execute-unit-tests-serially-rather-than-in-parallel

### Design Patterns

- Options Pattern docs: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-10.0
- Options Pattern example: https://medium.com/@vijaykr100/options-pattern-in-asp-net-core-7121e7bd5054
- Options Pattern troubleshooting: https://stackoverflow.com/questions/61352682/error-cs1503-cannot-convert-from-microsoft-extensions-configuration-iconfigura

### Tools & 3rd Party

- ERD: https://lucid.app/lucidchart/80f9e014-52b0-4936-90e0-51cf2d40980b/edit?viewport_loc=32%2C-11%2C892%2C1085%2C0_0&invitationId=inv_1ef36d86-f9aa-4297-9f12-42a1ae19f457
- Redis Insight: https://medium.com/@mahmud.ibrahim021/set-up-redis-with-redisinsight-using-docker-for-local-development-64b0c2aad4a7

## Contribution

1. Create feature branch from `develop`
2. Follow vertical slice pattern
3. Add tests for new features
4. Submit PR for review

## Copyright

© 2026 Nashtech. All rights reserved.

![Nashtech Logo](./images/nashtech_logo.png)
