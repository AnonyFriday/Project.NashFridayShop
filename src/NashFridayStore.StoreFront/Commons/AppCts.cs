namespace NashFridayStore.StoreFront.Commons;

internal static class AppCts
{
    public static class ToastType
    {
        public const string Success = "success";
        public const string Error = "error";
    }

    public static class Identity
    {
        public const string AuthenticationType = "BffAuth";
    }

    public static class Cookie
    {
        public const string BffCookieName = "NashFridayStore.BFF.Session";
        public const string IdentityCookieName = "NashFridayStore.Identity.LoginSession";
    }
}
