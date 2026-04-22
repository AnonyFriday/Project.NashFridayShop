namespace NashFridayStore.Domain.Commons;

public static class AppCts
{
    public static class Environment
    {
        public const string Testing = "Testing";
    }

    public static class Api
    {
        public const int MaxStars = 10;
        public const int MinStars = 1;
        public const int PageSize = 10;
        public const int PageIndex = 0;
    }

    public static class Policy
    {
        public const string AdminSite = "AdminSitePolicy";
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
    }
}
