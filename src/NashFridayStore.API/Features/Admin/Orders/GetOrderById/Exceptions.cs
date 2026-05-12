using FluentValidation.Results;
using NashFridayStore.API.Commons.Exceptions;
using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.API.Features.Admin.Orders.GetOrderById;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors)
        : AppValidationException(errors);

    public sealed class OrderNotFoundException(Guid id) : AppException(
        ProblemDetailsTypes.NotFound,
        "Order Not Found",
        $"Order with ID {id} was not found."
    );
}
