using FluentValidation.Results;
using FluentValidation.TestHelper;
using NashFridayStore.API.Features.Admin.Orders.GetOrderById;
using Xunit;

namespace NashFridayStore.UnitTests.Features.Admin.Orders;

public class GetOrderByIdValidatorTests
{
    private readonly Validator _validator = new();

    [Fact]
    [Trait("UT", "General")]
    public void Validate_IdIsEmpty_ShouldHaveValidationError()
    {
        var request = new Request(Guid.Empty);

        TestValidationResult<Request> result = _validator.TestValidate(request);

        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.Id), error.PropertyName);
        Assert.Equal(Validator.IdIsRequired, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "General")]
    public void Validate_IdIsNotEmpty_ShouldNotHaveValidationError()
    {
        var request = new Request(Guid.NewGuid());

        TestValidationResult<Request> result = _validator.TestValidate(request);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
