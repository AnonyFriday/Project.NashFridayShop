namespace NashFridayStore.Domain.Commons;

public static class AppCts
{
    public static class Environment
    {
        public const string Testing = "Testing";
    }

    public static class Currency
    {
        public const string Usd = "USD";
    }

    public static class Api
    {
        public const int MaxStars = 5;
        public const int MinStars = 1;
        public const int PageSize = 10;
        public const int PageIndex = 0;
        public const int ThresholdForNewProduct = 7;
    }

    public static class Cart
    {
        public const int CartimeToLiveInMinutes = 60;
    }

    public static class Policy
    {
        public const string AdminSite = "AdminSitePolicy";
    }

    public static class Identity
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Customer = "Customer";
        }
    }

    public static class ProblemDetailsTypes
    {
        public sealed record ProblemType(int StatusCode, string TypeLink);

        public static readonly ProblemType BadRequest =
            new(400, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1");

        public static readonly ProblemType NotFound =
            new(404, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4");

        public static readonly ProblemType Conflict =
            new(409, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8");

        public static readonly ProblemType Unauthorized =
            new(401, "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1");

        public static readonly ProblemType Forbidden =
            new(403, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3");

        public static readonly ProblemType UnsupportedMediaType =
            new(415, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.13");

        public static readonly ProblemType FileTooLarge =
            new(413, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.11");
    }
}
