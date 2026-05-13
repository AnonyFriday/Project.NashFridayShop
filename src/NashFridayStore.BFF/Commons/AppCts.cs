namespace NashFridayStore.BFF.Commons;

public static class AppCts
{
    public static class Policy
    {
        public const string AdminSite = "AdminSitePolicy";
    }

    public static class Auth
    {
        public const int TokenTimeToLiveInMinutes = 60;
        public const int CookieTimeToLiveInMinutes = 60;
    }
}
