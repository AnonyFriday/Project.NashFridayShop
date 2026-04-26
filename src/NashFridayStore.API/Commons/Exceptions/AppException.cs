using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.API.Commons.Exceptions;

public class AppException : Exception
{
    public int StatusCode { get; }
    public string Title { get; }
    public string TypeName { get; }

    protected AppException(ProblemDetailsTypes.ProblemType problem, string title, string message)
        : base(message)
    {
        StatusCode = problem.StatusCode;
        TypeName = problem.TypeLink;
        Title = title;
    }
}
