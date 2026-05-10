# Nash Friday Shop (R2E Training Bootcamp)

![codecov](https://codecov.io/gh/AnonyFriday/Project.NashFridayShop/graph/badge.svg?token=GU9DBWMKZZ)

## Techstack

- **🔧 Backend**: .NET 10 (ASP.NET Core Web Api, Entity Framework Core, FluentValidation)
- **🌐 Frontend**: Next.js (Admin Site), ASP.NET Razor Pages (StoreFront), HTMX
- **🧩 BFF**: OIDC client, YARP Reverse Proxy, Cookie-based authentication
- **🔐 Identity**: OpenIddict, Authorization Code Flow + PKCE
- **🗄️ Database**: SQL Server
- **🏗️ Architecture**: Vertical Slice Architecture
- **🧪 Testing**: xUnit, Coverlet
- **🛠️ Tooling**: Directory.Packages.props, Directory.Build.props, docker-compose.yaml
- **🎪 Payment**: Stripe
- **🛒 Cart**: Redis

## Current Supporting APIs

| Layer               | Endpoint                                 | Method   | Description                                             | Status                | Tests     |
| ------------------- | ---------------------------------------- | -------- | ------------------------------------------------------- | --------------------- | --------- |
| **Public API**      | `/api/all/categories`                    | GET      | List all categories                                     | ✅ Completed          | ✅ UT, IT |
| **Public API**      | `/api/all/categories/{id}`               | GET      | Category details                                        | ✅ Completed          | ✅ UT, IT |
| **Public API**      | `/api/all/products`                      | GET      | Product listing, filters, pagination                    | ✅ Completed          | ✅ UT, IT |
| **Public API**      | `/api/all/products/{id}`                 | GET      | Product details                                         | ✅ Completed          | ✅ UT, IT |
| **Public API**      | `/api/all/products/{id}/ratings`         | GET      | Product ratings list                                    | ✅ Completed          | ✅ UT, IT |
| **Customer API**    | `/api/customer/products/{id}/rating`     | POST     | Add product rating/comment                              | ✅ Completed          | ✅ UT, IT |
| **Customer API**    | `/api/customer/cart`                     | GET      | Get current user cart                                   | ✅ Completed          | ❌ None   |
| **Customer API**    | `/api/customer/cart`                     | POST     | Create or add item to cart or update product's quantity | ✅ Completed          | ❌ None   |
| **Admin API**       | `/api/admin/products`                    | GET/POST | List products / Create product                          | ✅ Completed          | ✅ UT, IT |
| **Admin API**       | `/api/admin/products/{id}`               | GET/PUT  | Product details / Update product                        | ✅ Completed          | ✅ UT, IT |
| **Admin API**       | `/api/admin/products/{id}/toggle-delete` | POST     | Soft delete (toggle) product                            | ✅ Completed          | ✅ UT, IT |
| **Admin API**       | `/api/admin/products/{id}/image`         | POST     | Update product image                                    | ✅ Completed          | ✅ UT, IT |
| **Admin API**       | `/api/admin/categories`                  | GET/POST | List categories / Create category                       | ✅ Completed          | ✅ UT, IT |
| **Admin API**       | `/api/admin/categories/{id}`             | GET/PUT  | Category details / Update category                      | ✅ Completed          | ✅ UT, IT |
| **Admin API**       | `/api/admin/orders`                      | GET      | Order listing                                           | ❌ Not implemented    | ❌ None   |
| **Identity Admin**  | `/api/admin/customers/{id}`              | DELETE   | Disable customer                                        | ❌ Not implemented    | ❌ None   |
| **Identity Admin**  | `/api/admin/customers`                   | GET      | Customer listing (Paginated & Searchable)               | ✅ Completed          | ❌ None   |
| **Identity Server** | `/connect/authorize`                     | GET      | Start OIDC authorization code flow                      | ✅ Completed          | ❌ None   |
| **Identity Server** | `/connect/token`                         | POST     | Exchange auth code → tokens                             | ✅ Completed          | ❌ None   |
| **Identity Server** | `/connect/logout`                        | POST     | Identity logout flow                                    | ✅ Completed          | ❌ None   |
| **BFF**             | `/api/auth/login`                        | GET      | Start login from React → IdentityServer                 | ✅ Completed          | ❌ None   |
| **BFF**             | `/api/auth/me`                           | GET      | Get current user info & claims                          | ✅ Completed          | ❌ None   |
| **BFF**             | `/api/auth/logout`                       | POST     | Logout BFF session + Identity session                   | ✅ Completed          | ❌ None   |
| **BFF**             | `/signin-oidc`                           | GET      | OIDC callback endpoint (middleware handled)             | ✅ Middleware handled | ❌ None   |
| **BFF**             | `/dev/auth/tokens`(Dev Only)             | GET      | Return access_token, id_token, refresh_token            | ✅ Completed          | ❌ None   |
| **BFF**             | `/api/{**catch-all}`                     | ALL      | Reverse proxy Customer-site + Admin-site requests → API | ✅ Completed          | ❌ None   |

## Current Supporting Pages In Admin, Customer and Identity Server

| Layer               | Endpoint                 | Method | Description                    | Status             | Tests   |
| ------------------- | ------------------------ | ------ | ------------------------------ | ------------------ | ------- |
| **Admin Site**      | `/dashboard`             | GET    | Admin overview & statistics    | ❌ Not implemented | ❌ None |
| **Admin Site**      | `/products`              | GET    | Product management list        | ✅ Completed       | ❌ None |
| **Admin Site**      | `/products/new`          | GET    | Create new product page        | ✅ Completed       | ❌ None |
| **Admin Site**      | `/products/[id]`         | GET    | Edit product details page      | ✅ Completed       | ❌ None |
| **Admin Site**      | `/categories`            | GET    | Category management list       | ✅ Completed       | ❌ None |
| **Admin Site**      | `/categories/new`        | GET    | Create new category page       | ✅ Completed       | ❌ None |
| **Admin Site**      | `/categories/[id]`       | GET    | Edit category details page     | ✅ Completed       | ❌ None |
| **Admin Site**      | `/customers`             | GET    | Customer management list       | ✅ Completed       | ❌ None |
| **Admin Site**      | `/orders`                | GET    | Order management list          | ❌ Not implemented | ❌ None |
| **StoreFront**      | `/`                      | GET    | Home Page (Top Rated Products) | ✅ Completed       | ❌ None |
| **StoreFront**      | `/Products`              | GET    | Product Search & Filter Page   | ✅ Completed       | ❌ None |
| **StoreFront**      | `/Products/Details/{id}` | GET    | Product Details Page           | ✅ Completed       | ❌ None |
| **StoreFront**      | `/Cart`                  | GET    | Shopping Cart Page             | ✅ Completed       | ❌ None |
| **StoreFront**      | `/Errors/{code}`         | GET    | Global Error Pages (404, 500)  | ✅ Completed       | ❌ None |
| **Identity Server** | `/Account/Login`         | GET    | Render Razor login page        | ✅ Completed       | ❌ None |
| **Identity Server** | `/Account/Login`         | POST   | Submit login credentials       | ✅ Completed       | ❌ None |
| **Identity Server** | `/Account/Register`      | GET    | Render registration page       | ❌ Not implemented | ❌ None |
| **Identity Server** | `/Account/Register`      | POST   | Submit registration form       | ❌ Not implemented | ❌ None |

## ERD (V1)

![ERD Diagram](./images/erd_v1_no_identity.png)

## BFF + Reverse Proxy, Identity Server, API Server, Frontends communications

![BFF and Identity Server Communication](./images/BFF_IdentityServer.png)

## Week 1-2-3-4 Summary

### Week 1-2 Highlights

- **Infra**: Setup Docker (SQL Server, Redis), CI Pipelines, and Vertical Slice Architecture.
- **Testing**: Integrated xUnit & SQLite In-Memory for API integration tests.
- **Backend**: Built API Server & Identity Server (OpenIddict + Auth Code Flow + PKCE).
- **Security**: Implemented BFF Pattern with YARP Reverse Proxy & Cookie sessions.
- **Frontend**: Scaffolded Admin Portal (Next.js) & StoreFront (Razor Pages).

### Week 3 Highlights

- **BE**: Added Quantity & Ratings to Products.
- **BE**: Implemented Soft Delete (Toggle).
- **BE**: Added searchable Customer list in Identity Server.
- **Auth**: Integrated full BFF Login/Session flow in Admin Portal.
- **FE**: Built Admin Portal (Next.js 15) with Product & Category CRUD.

### Week 4 Highlights (Current)

- **FE**: Developed **Resilient StoreFront Architecture**:
  - Centralized Error Handling (404/500) with clean routing.
  - Global Toast Notification System (DaisyUI + htmx).
  - htmx trigger merging.
  - API Safety Guard: Automatic backend error-to-toast mapping.
- **Cart**: Implemented real-time Cart interactions (Add/Update/Remove) with navbar syncing.
- **Admin**: Built modern Admin Portal (Next.js 16) with Product & Category CRUD.
- **Admin**: Implemented product image management (API & UI).
- **Auth**: Refactored authentication middleware for safe OIDC redirects with PKCE.
- **Docs**: Comprehensive Reference Index for htmx, OIDC, and Modern .NET patterns.
- **Pending**: Dashboard (Statistics) and Checkout flow (Stripe).

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

### 🌐 Frontend Stack & CI/CD

- **React CI/CD**: [GitHub Actions for React](https://santhosh-adiga-u.medium.com/setting-up-a-complete-ci-cd-pipeline-for-react-using-github-actions-9a07613ceded)
- **Deployment**: [Vercel](https://vercel.com)
- **Testing**: [Jest (Unit/Integration)](https://jestjs.io/), [Cypress (E2E)](https://www.cypress.io/)

### 🛡️ htmx & UI

- **htmx Guide**: [htmx for ASP.NET Developers](https://aspnet-htmx.com/chapter05/)
- **hx-trigger**: [Custom Event Triggers](https://htmx.org/headers/hx-trigger/)
- **UI Frameworks**: [daisyUI](https://daisyui.com), [tailwindcss](https://tailwindcss.com)

### 🧩 ASP.NET Core Razor Pages

- **ViewComponents**: [Reusable UI logic](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/view-components)
- **Tag Helpers**: [Native HTML enhancements](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro)
- **Middleware**: [Request Pipeline & Delegates](https://medium.com/@Sina-Riyahi/understanding-request-delegates-and-middleware-in-asp-net-core-5f9b22d16613)
- **Request Storage**: [HttpContext.Items (Request Scope)](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext.items)

### 🏗️ Architecture & Backend

- **Vertical Slice Architecture**: [The Jimmy Bogard Pattern](https://nadirbad.dev/vertical-slice-architecture-dotnet)
- **Modern Exception Handling**: [Global Error Handling in .NET 8+](https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8)
- **ProblemDetails**: [RFC 7807 Implementation](https://medium.com/@aseem2372005/handling-api-errors-the-right-way-understanding-problemdetails-in-asp-net-core-web-api-e3f7d404672c)
- **DbContext Pooling**: [Efficiency at scale](https://medium.com/@razeshmsb02/adddbcontext-vs-adddbcontextpool-vs-adddbcontextfactory-3760857737d1)
- **HATEOAS**: [Self-Discoverable APIs](https://dev.to/wallacefreitas/supercharge-your-rest-apis-with-hateoas-the-key-to-smarter-self-discoverable-endpoints-5dg3)
- **Delegating Handlers**: [Extending HttpClient](https://www.milanjovanovic.tech/blog/extending-httpclient-with-delegating-handlers-in-aspnetcore)

### 🔐 Identity & Security

- **BFF Pattern**: [Backend For Frontend (Auth0)](https://auth0.com/blog/the-backend-for-frontend-pattern-bff/)
- **IdentityServer**: [Duende Big Picture](https://docs.duendesoftware.com/identityserver/overview/big-picture/)
- **OpenIddict**: [Auth Code Flow + PKCE](https://dev.to/naeemsahil/implementing-openid-connect-with-openiddict-4fmp)
- **OIDC Claims**: [ID Token Standard](https://openid.net/specs/openid-connect-core-1_0.html#IDToken)
- **JWT Auth**: [Configure Bearer Auth](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-jwt-bearer-authentication)

### 🗄️ Database & Storage

- **SQL Server**: [Official Docker Image](https://hub.docker.com/r/microsoft/mssql-server)
- **Inheritance Mapping**: [TPH, TPT, TPC Patterns](https://medium.com/@sematopcu/inheritance-mapping-in-databases-tph-tpt-tpc-fc175c572880)
- **Cloud Storage**: [Google Cloud Storage Buckets](https://docs.cloud.google.com/storage/docs/creating-buckets)
- **Auth Storage**: [Application Default Credentials (ADC)](https://docs.cloud.google.com/docs/authentication/application-default-credentials)

### 🧪 Testing

- **Integration Tests**: [ASP.NET Core Guide](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
- **In-Memory DBs**: [SQLite In-Memory Pros/Cons](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/in-memory-databases)
- **Serial Execution**: [Forcing serial tests in xUnit](https://stackoverflow.com/questions/1408175/execute-unit-tests-serially-rather-than-in-parallel)

### 🛠️ Tooling & Infrastructure

- **Reverse Proxy**: [YARP (Yet Another Reverse Proxy)](https://github.com/dotnet/yarp)
- **Redis Insight**: [Docker Setup for Redis Visualization](https://medium.com/@mahmud.ibrahim021/set-up-redis-with-redisinsight-using-docker-for-local-development-64b0c2aad4a7)
- **ERD**: [LucidChart Diagram](https://lucid.app/lucidchart/80f9e014-52b0-4936-90e0-51cf2d40980b/edit)

## Contribution

1. Create feature branch from `develop`
2. Follow vertical slice pattern
3. Add tests for new features
4. Submit PR for review

## Copyright

© 2026 Nashtech. All rights reserved.

![Nashtech Logo](./images/nashtech_logo.png)
