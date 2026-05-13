using static NashFridayStore.IdentityServer.Commons.AppCts;

namespace NashFridayStore.IdentityServer.Commons.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException()
        : base(ProblemDetailsTypes.Forbidden, title: "Forbidden", message: "User do not have permission to access this page")
    {
    }
}