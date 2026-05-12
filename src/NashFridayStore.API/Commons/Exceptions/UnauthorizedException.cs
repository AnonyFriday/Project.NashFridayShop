using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.API.Commons.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException()
        : base(ProblemDetailsTypes.Unauthorized, title: "Token is invalid or expired", message: "Please login to continue")
    {
    }
}