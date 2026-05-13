using FluentValidation.Results;
using NashFridayStore.API.Commons.Exceptions;

namespace NashFridayStore.API.Features.Customer.Cart.CreateOrAddItemToCart;

public static class Exceptions
{
    public class ValidationException(IEnumerable<ValidationFailure> failures)
        : AppValidationException(failures);
}
