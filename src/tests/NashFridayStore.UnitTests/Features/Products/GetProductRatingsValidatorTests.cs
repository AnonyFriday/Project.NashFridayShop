using FluentValidation.Results;
using FluentValidation.TestHelper;
using NashFridayStore.API.Features.Products.GetProductRatings;

namespace NashFridayStore.UnitTests.Features.Products;

public class GetProductRatingsValidatorTests
{
    private readonly Validator _validator;

    public GetProductRatingsValidatorTests()
    {
        _validator = new Validator();
    }

    #region ProductId Tests
    [Fact]
    [Trait("UT", "ProductId")]
    public void Validate_ProductIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.Empty, 0, 10);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.ProductId), error.PropertyName);
        Assert.Equal("Product Id is required.", error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "ProductId")]
    public void Validate_ProductIdIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), 0, 10);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region PageIndex Tests
    [Fact]
    [Trait("UT", "PageIndex")]
    public void Validate_PageIndexIsNegative_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), -1, 10);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.PageIndex), error.PropertyName);
        Assert.Equal(Validator.PageIndexGreaterThanOrEqualZero, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "PageIndex")]
    public void Validate_PageIndexIsPositive_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), 5, 10);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region PageSize Tests
    [Fact]
    [Trait("UT", "PageSize")]
    public void Validate_PageSizeIsLessThanMinimum_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), 0, 0);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.PageSize), error.PropertyName);
        Assert.Equal(Validator.PageSizeBetweenRange, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "PageSize")]
    public void Validate_PageSizeIsGreaterThanMaximum_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), 0, 101);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.PageSize), error.PropertyName);
        Assert.Equal(Validator.PageSizeBetweenRange, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "PageSize")]
    public void Validate_PageSizeIsMinimum_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), 0, 1);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    [Trait("UT", "PageSize")]
    public void Validate_PageSizeIsMaximum_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), 0, 100);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    [Trait("UT", "PageSize")]
    public void Validate_PageSizeIsWithinRange_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), 0, 50);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region Passed All Tests
    [Fact]
    [Trait("UT", "General")]
    public void Validate_RequestIsValid_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), 0, 10);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    [Trait("UT", "General")]
    public void Validate_RequestHasMultipleErrors_ShouldHaveAllValidationErrors()
    {
        // Arrange
        var request = new Request(Guid.Empty, -1, 101);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);

        Assert.Contains(result.Errors, e =>
            e.PropertyName == nameof(Request.ProductId) &&
            e.ErrorMessage == "Product Id is required.");

        Assert.Contains(result.Errors, e =>
            e.PropertyName == nameof(Request.PageIndex) &&
            e.ErrorMessage == Validator.PageIndexGreaterThanOrEqualZero);

        Assert.Contains(result.Errors, e =>
            e.PropertyName == nameof(Request.PageSize) &&
            e.ErrorMessage == Validator.PageSizeBetweenRange);
    }
    #endregion
}
