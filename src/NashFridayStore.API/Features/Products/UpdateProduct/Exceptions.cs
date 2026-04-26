using FluentValidation.Results;
using NashFridayStore.API.Commons.Exceptions;
using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.API.Features.Products.UpdateProduct;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);

    public sealed class ProductNotFoundException(Guid id) : AppException(
        ProblemDetailsTypes.NotFound,
        "Product Not Found",
        $"Product with ID '{id}' was not found"
    )
    {
        public Guid ProductId { get; } = id;
    }

    public sealed class CategoryNotFoundException(Guid categoryId) : AppException(
        ProblemDetailsTypes.NotFound,
        "Category Not Found",
        $"Category with ID '{categoryId}' was not found"
    )
    {
        public Guid CategoryId { get; } = categoryId;
    }
}
