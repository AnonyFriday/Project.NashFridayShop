using FluentValidation.Results;
using FluentValidation.TestHelper;
using NashFridayStore.API.Features.Admin.Orders.GetOrders;
using Xunit;

namespace NashFridayStore.UnitTests.Features.Admin.Orders;

public class GetOrdersValidatorTests
{
    private readonly Validator _validator = new();

    #region Paging Tests
    [Fact]
    [Trait("UT", "Paging")]
    public void Validate_PageIndexIsNegative_ShouldHaveValidationError()
    {
        var request = new Request(PageIndex: -1);

        TestValidationResult<Request> result = _validator.TestValidate(request);

        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.PageIndex), error.PropertyName);
        Assert.Equal(Validator.PageIndexGreaterThanOrEqualZero, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Paging")]
    public void Validate_PageSizeIsNegative_ShouldHaveValidationError()
    {
        var request = new Request(PageSize: -1);

        TestValidationResult<Request> result = _validator.TestValidate(request);

        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.PageSize), error.PropertyName);
        Assert.Equal(Validator.PageSizeGreaterThanOrEqualZero, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Paging")]
    public void Validate_PagingIsValid_ShouldNotHaveValidationError()
    {
        var request = new Request(PageIndex: 0, PageSize: 10);

        TestValidationResult<Request> result = _validator.TestValidate(request);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    #endregion

    #region General Tests
    [Fact]
    [Trait("UT", "General")]
    public void Validate_RequestWithFiltersIsValid_ShouldNotHaveAnyValidationErrors()
    {
        var request = new Request(0, 10, Domain.Entities.Orders.OrderStatus.Completed, Domain.Entities.Orders.PaymentStatus.Paid);

        TestValidationResult<Request> result = _validator.TestValidate(request);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    #endregion
}
