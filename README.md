# Nash Friday Shop 
(R2E Training Bootcamp)

![codecov](https://codecov.io/gh/AnonyFriday/Project.NashFridayShop/graph/badge.svg?token=GU9DBWMKZZ)

## Techstack

- **🔧 Backend**: .NET 10 (ASP.NET Core Web Api, Entity Framework Core, FluentValidation)
- **🌐 Frontend**: Next.js (Admin Site), ASP.NET Razor Pages (StoreFront)
- **🧩 BFF**: OIDC client, YARP Reverse Proxy, Cookie-based authentication
- **🔐 Identity**: OpenIddict, Authorization Code Flow + PKCE
- **🗄️ Database**: SQL Server
- **🏗️ Architecture**: Vertical Slice Architecture
- **🧪 Testing**: xUnit, Coverlet
- **🛠️ Tooling**: Directory.Packages.props, Directory.Build.props, docker-compose.yaml
- **🎪 Payment**: Stripe
- **🛒 Cart**: Redis

## Current Supporting APIs

| Layer           | Endpoint                     | Method | Description                                         | Status                | Tests     |
| --------------- | ---------------------------- | ------ | --------------------------------------------------- | --------------------- | --------- |
| API             | `/api/categories`            | GET    | Category menu / list categories                     | ✅ Completed          | ✅ UT, IT |
| API             | `/api/categories/{id}`       | GET    | Category details                                    | ✅ Completed          | ✅ UT, IT |
| API             | `/api/products`              | GET    | Product listing, filters, pagination                | ✅ Completed          | ✅ UT, IT |
| API             | `/api/products/{id}`         | GET    | Product details                                     | ✅ Completed          | ✅ UT, IT |
| API             | `/api/products`              | POST   | Create product                                      | ✅ Completed          | ✅ UT, IT |
| API             | `/api/products/{id}`         | PUT    | Update product                                      | ✅ Completed          | ✅ UT, IT |
| API             | `/api/products/{id}`         | DELETE | Soft delete product                                 | ✅ Completed          | ✅ UT, IT |
| API             | `/api/products/{id}/ratings` | GET    | Product ratings list                                | ✅ Completed          | ✅ UT, IT |
| API             | `/api/products/{id}/rating`  | POST   | Add rating/comment                                  | ✅ Completed          | ✅ UT, IT |
| API             | `/api/orders`                | GET    | Order listing                                       | ❌ Not implemented    | ❌ None   |
| API             | `/api/customers`             | GET    | Customer listing                                    | ❌ Not implemented    | ❌ None   |
| API             | `/api/customers/{id}`        | DELETE | Disable customer                                    | ❌ Not implemented    | ❌ None   |
| Identity Server | `/connect/authorize`         | GET    | Start authorization code flow                       | ✅ Completed          | ❌ None   |
| Identity Server | `/connect/token`             | POST   | Exchange auth code → tokens                         | ✅ Completed          | ❌ None   |
| Identity Server | `/connect/logout`            | POST   | Identity logout flow                                | ✅ Completed          | ❌ None   |
| BFF             | `/api/auth/login`            | GET    | Start login from React → redirect to IdentityServer | ✅ Completed          | ❌ None   |
| BFF             | `/apit/auth/logout`          | POST   | Logout BFF session + Identity session               | ✅ Completed          | ❌ None   |
| BFF             | `/signin-oidc`               | GET    | OIDC callback endpoint (middleware handled)         | ✅ Middleware handled | ❌ None   |
| BFF             | `/dev/auth/tokens`(Dev Only) | GET    | Return access_token, id_token, refresh_token | ✅ Completed          | ❌ None   |
| BFF             | `/api/{**catch-all}`         | ALL    | Reverse proxy React requests → API                  | ✅ Completed          | ❌ None   |

## Current Supporting Pages For Identity Server

| Layer           | Endpoint            | Method | Description              | Status             | Tests   |
| --------------- | ------------------- | ------ | ------------------------ | ------------------ | ------- |
| Identity Server | `/Account/Login`    | GET    | Render Razor login page  | ✅ Completed       | ❌ None |
| Identity Server | `/Account/Login`    | POST   | Submit login credentials | ✅ Completed       | ❌ None |
| Identity Server | `/Account/Register` | GET    | Render registration page | ❌ Not implemented | ❌ None |
| Identity Server | `/Account/Register` | POST   | Submit registration form | ❌ Not implemented | ❌ None |

## ERD (V1)

![ERD Diagram](./images/erd_v1_no_identity.png)

## BFF + Reverse Proxy, Identity Server, API Server, Frontends communications

![BFF and Identity Server Communication](./images/BFF_IdentityServer.png)

## Week 1-2-3 Summary

### Project Foundation & Environment

- **Development tooling setup**: Fork, Postman, VS Code, Docker Desktop, SSMS
- **Environment configuration**: User Secrets, appsettings.Development.json, IOptions

### Testing Infrastructure

- xUnit
- SQLite In-Memory
- WebApplicationFactory
- Code coverage setup

### Frontend & Services

- Next.js Admin Site scaffold
- Razor Pages StoreFront scaffold
- BFF storing tokens + YARP Reverse Proxy
- Identity Server project + seperate Database
- API Server

### Authentication & Authorization

- Authorization Code Flow + PKCE
- Access Token
- ID Token
- Refresh Token
- Scope-based claim issuance
- Cookie-based BFF session

### CI/CD and Services

- Next.JS CI Pipeline
- .NET CI pipeline
- CD Pipeline (later)
- Docker Compose for images: Redis, SQL Server, Redis Insight

## Project Structure

```
src/
├── NashFridayStore.API/              # Endpoints, Validations, Handlers, Exceptions
├── NashFridayStore.Domain/           # Domain entities
├── NashFridayStore.Infrastructure/   # Data access, configurations, migrations
├── NashFridayStore.BFF/              # Storing Tokens, Reverse Proxy
├── NashFridayStore.IdentityServer/   # Auth, Authz service
├── NashFridayStore.StoreFront/       # Frontend Customer-site
├── admin-site/                       # Frontend Admin-site
└── tests/                            # Unit and integration tests
```

## Vertical Slice Architecture Explanation

Each feature is organized in its own "slice" within the **API** project with all related business logic together:

- **Request**: Request contract object
- **Response**: Response contract returned by the handler
- **Handler**: Core business logic and domain operations
- **Validator**: FluentValidation rules for requests
- **Exceptions**: Custom exceptions for the feature
- **Endpoint**: THe API Endpoint based on Controller

Example from `src/NashFridayStore.API/Features/Products/GetProduct/`:

**Request.cs**:
```csharp
public sealed record Request(Guid Id);
```

**Response.cs**:
```csharp
public sealed record Response(Guid Id, string Name, string ImageUrl, decimal PriceUsd, ProductStatus Status);
```

**Validator.cs**:
```csharp
public sealed class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product Id is required.");
    }
}
```

**Handler.cs**:
```csharp
public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
    {
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw Exceptions.Validation(validation.Errors);
        }

        Response? product = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.Id == req.Id)
            .Select(x => new Response(x.Id, x.Name, x.ImageUrl, x.PriceUsd, x.Status))
            .FirstOrDefaultAsync(ct);

        if (product is null)
        {
            throw Exceptions.NotFound(req.Id);
        }

        return product;
    }
}
```

**Exceptions.cs**:
```csharp
internal static class Exceptions
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

**Endpoint.cs**:
```csharp
using NashFridayStore.API.Features.Products.GetProduct;

[ApiController]
[Route("api/products/{id:guid}")]
public sealed class GetProductEndpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new Request(id);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
```

**Unit Test**:
```csharp
[Fact]
[Trait("UT", "Id")]
public void Validate_IdIsEmpty_ShouldHaveValidationError()
{
    // Arrange
    var request = new Request(Guid.Empty);

    // Act
    TestValidationResult<Request> result = _validator.TestValidate(request);

    // Assert
    Assert.False(result.IsValid);
    ValidationFailure error = Assert.Single(result.Errors);
    Assert.Equal(nameof(Request.Id), error.PropertyName);
    Assert.Equal(Validator.IdRequired, error.ErrorMessage);
}
``` 

**Integration Test**:
```csharp
[Fact]
public async Task GetProduct_ById_ShouldReturnProduct()
{
    // Arrange
    CancellationToken cancellationToken = TestContext.Current.CancellationToken;
    Category category = new CategoryBuilder().Build();

    Product product = new ProductBuilder()
        .WithCategoryId(category.Id)
        .WithName("Laptop")
        .Build();

    _dbContext.Categories.Add(category);
    _dbContext.Products.Add(product);
    await _dbContext.SaveChangesAsync(cancellationToken);

    // Act
    HttpResponseMessage response = await _client.GetAsync($"/api/products/{product.Id}", cancellationToken);

    // Assert
    response.EnsureSuccessStatusCode();
    Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

    Assert.NotNull(result);
    Assert.Equal(product.Id, result!.Id);
    Assert.Equal("Laptop", result.Name);
    Assert.Equal(product.PriceUsd, result.PriceUsd);
    Assert.Equal(product.Status, result.Status);
}
```

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

### Backend For Frontend (BFF)

- Auth0 BFF guide: https://auth0.com/blog/the-backend-for-frontend-pattern-bff/
- BFF explanation video: https://www.youtube.com/watch?v=hWJuX-8Ur2k

### Reverse Proxy YARP

- How to setup: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/yarp-overview?view=aspnetcore-10.0
- Project YARP: https://github.com/dotnet/yarp

### Identity Server

- Duende docs: https://docs.duendesoftware.com/
- IdentityServer docs: https://docs.duendesoftware.com/identityserver/
- IdentityServer overview: https://docs.duendesoftware.com/identityserver/overview/big-picture/

### OpenIddict

- OpenIddict guide: https://dev.to/naeemsahil/implementing-openid-connect-with-openiddict-4fmp
- Client credentials flow: https://dev.to/robinvanderknaap/setting-up-an-authorization-server-with-openiddict-part-iii-client-credentials-flow-55lp
- PAR configuration: https://documentation.openiddict.com/configuration/pushed-authorization-requests#allowing-client-applications-to-use-the-pushed-authorization-endpoint

### OAuth2 / OpenID Connect

- OAuth2 + OIDC video: https://www.youtube.com/watch?v=uUxD1uF244E
- OAuth2 article: https://viblo.asia/p/tim-hieu-doi-chut-ve-oauth2-eW65GvMLlDO
- OAuth2 RFC: https://datatracker.ietf.org/doc/html/rfc6749#section-1.1
- Required OIDC claims: https://openid.net/specs/openid-connect-core-1_0.html#IDToken

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
