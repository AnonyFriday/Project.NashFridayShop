# Nash Friday Shop

![codecov](https://codecov.io/gh/AnonyFriday/Project.NashFridayShop/graph/badge.svg?token=GU9DBWMKZZ)

## Techstack

- **Backend**: .NET 10 (ASP.NET Core, Entity Framework Core, FluentValidation)
- **Frontend**: Next.js (Admin Site), ASP.NET Razor Pages (StoreFront)
- **BFF**: Backend for Frontend service
- **Identity**: IdentityServer4
- **Database**: SQL Server (via EF Core)
- **Architecture**: Vertical Slice Architecture
- **Testings**: Unit Testing, Integration Testing

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

## Backend Libraries Installed

- Microsoft.AspNetCore (Web API)
- Microsoft.EntityFrameworkCore (ORM)
- FluentValidation (Validation)
- Microsoft.AspNetCore.Authentication.JwtBearer (Auth)
- Scalar (API Documentation)
- Coverlet (Test Coverage)
- xunit (Testing)

## Configuration & Tools

- Central package management: `Directory.Packages.props`
- Global coding analyzer: `Directory.Build.props`
- Coding style: `.editorconfig`

## ERD (V1)

![ERD Diagram](./images/erd_v1_no_identity.png)

## Week 1 Summary

- Setup FE + BE source code
  - Dev environment: fork, Postman, secrets, `appsettings.Development.json`, `IOptions`
  - Coding convention: `.editorconfig`
  - Central package management: `Directory.Packages.props`
  - Global analyzer: `Directory.Build.props`
  - SonarAnalyzer
  - DbContext
  - Razor Pages, BFF, Identity Server (Currently template only)
  - NextJs (Currently template only)
  - CORS
  - Code coverage
  - HTTPS: `mkcert`, `dotnet dev-certs`, Next.js dev cert
  - Documentation: Scalar
  - API versioning
  - Testing: xUnit, SQLite, WebApplicationFactory
  - Exception handler: API exception, validation exception, general exception handler
- Setup CI/CD and other services
  - CI for FE, CI for BE
  - Docker Compose: Redis, SQL Server, Redis Insight
- Research: Identity service, BFF, Vertical Slice Architecture
- Setup entity + configuration + migrations + seeder for Product and Category

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
├── StoreFront/                       # Customer-facing ASP.NET Razor Pages app
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

This keeps features isolated and easy to maintain.

## Contribution

1. Create feature branch from `develop`
2. Follow vertical slice pattern
3. Add tests for new features
4. Submit PR for review

## Copyright

© 2026 Nashtech. All rights reserved.
