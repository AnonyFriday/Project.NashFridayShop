using FluentValidation.Results;
using NashFridayStore.API.Commons.Exceptions;
using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.API.Features.Products.CreateProduct;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);

    public sealed class CategoryNotFoundException(Guid categoryId) : AppException(
        ProblemDetailsTypes.NotFound,
        "Category Not Found",
        $"Category with ID '{categoryId}' was not found"
    );
}
