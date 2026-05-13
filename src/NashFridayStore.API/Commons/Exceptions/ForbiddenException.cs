using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.API.Commons.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException()
        : base(ProblemDetailsTypes.Forbidden, title: "Forbidden", message: "User do not have permission to access this page")
    {
    }
}