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
}
