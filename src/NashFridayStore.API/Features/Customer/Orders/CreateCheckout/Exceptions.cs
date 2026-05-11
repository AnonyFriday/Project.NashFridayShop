using FluentValidation.Results;
using NashFridayStore.API.Commons.Exceptions;
using static NashFridayStore.Domain.Commons.AppCts;

namespace NashFridayStore.API.Features.Customer.Orders.CreateCheckout;

public static class Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidationFailure> errors) : AppValidationException(errors);

    public sealed class ProductNotFoundException(Guid productId) : AppException(
        ProblemDetailsTypes.NotFound,
        "Product Not Found",
        $"Product with ID '{productId}' was not found"
    );

    public sealed class ProductNotEnoughStockException(string productName, int requestedQuantity, int availableStock) : AppException(
        ProblemDetailsTypes.Conflict,
        "Product Not Enough Stock",
        $"Product '{productName}' has not enough stock. Requested: {requestedQuantity}, Available: {availableStock}"
    );

    public sealed class ProductOutOfStockException(string productName) : AppException(
        ProblemDetailsTypes.Conflict,
        "Product Out Of Stock",
        $"Product '{productName}' is out of stock."
    );

    public sealed class ProductIsDiscontinuedException(string productName) : AppException(
        ProblemDetailsTypes.Conflict,
        "Product Discontinued",
        $"Product '{productName}' is no longer available."
    );

    public sealed class CartIsEmptyException() : AppException(
        ProblemDetailsTypes.Conflict,
        "Cart Is Empty",
        "Cart is empty. Please add products to the cart before creating an order."
    );

    public sealed class ProductPriceOrNameChangedException(string productNameInCart) : AppException(
        ProblemDetailsTypes.Conflict,
        "Product Price Or Name Changed",
        $"Product '{productNameInCart}' has price or name changed. Please update your cart."
    );
}
