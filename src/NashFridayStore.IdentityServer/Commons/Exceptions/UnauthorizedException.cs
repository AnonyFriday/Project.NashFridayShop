using static NashFridayStore.IdentityServer.Commons.AppCts;

namespace NashFridayStore.IdentityServer.Commons.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException()
        : base(ProblemDetailsTypes.Unauthorized, title: "Token is invalid or expired", message: "Token is invalid or expired")
    {
    }
}