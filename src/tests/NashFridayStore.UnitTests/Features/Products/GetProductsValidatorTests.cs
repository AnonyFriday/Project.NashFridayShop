using FluentValidation.Results;
using FluentValidation.TestHelper;
using NashFridayStore.API.Features.Products.GetProducts;
using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.UnitTests.Features.Products;

public class GetProductsValidatorTests
{
    private readonly Validator _validator;

    public GetProductsValidatorTests()
    {
        _validator = new Validator();
    }

    #region SearchName Tests
    [Fact]
    [Trait("UT", "SearchName")]
    public void Validate_SearchNameExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(null, new string('a', 101), null, null);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.SearchName), error.PropertyName);
        Assert.Equal(Validator.SearchNameMaxLength, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "SearchName")]
    public void Validate_SearchNameEqualsMaxLength_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(null, new string('a', 100), null, null);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    [Trait("UT", "SearchName")]
    public void Validate_SearchNameIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(null, "Laptop", null, null);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region Price Tests
    [Fact]
    [Trait("UT", "Price")]
    public void Validate_MinPriceIsNegative_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(null, null, -1, 10);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.MinPrice), error.PropertyName);
        Assert.Equal(Validator.MinPriceGreaterThanOrEqualZero, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Price")]
    public void Validate_MaxPriceIsNegative_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(null, null, 20, -1);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        Assert.Contains(result.Errors, e =>
              e.PropertyName == nameof(Request.MaxPrice) &&
              e.ErrorMessage == Validator.MaxPriceGreaterThanOrEqualZero);
    }

    [Fact]
    [Trait("UT", "Price")]
    public void Validate_MinPriceGreaterThanMaxPrice_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(null, null, 100, 10);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(Validator.MinPriceLessThanOrEqualMaxPrice, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Price")]
    public void Validate_MinPriceLessThanOrEqualMaxPrice_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(null, null, 10, 100);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region Paging Tests
    [Fact]
    [Trait("UT", "Paging")]
    public void Validate_PageIndexIsNegative_ShouldHaveValidationError()
    {
        var request = new Request(null, null, null, null, PageIndex: -1);

        TestValidationResult<Request> result = _validator.TestValidate(request);

        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.PageIndex), error.PropertyName);
        Assert.Equal(Validator.PageIndexGreaterThanOrEqualZero, error.ErrorMessage);
    }


    [Fact]
    [Trait("UT", "Paging")]
    public void Validate_PageSizeOutOfRange_ShouldHaveValidationError()
    {
        var request = new Request(null, null, null, null, PageSize: 101);

        TestValidationResult<Request> result = _validator.TestValidate(request);

        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.PageSize), error.PropertyName);
        Assert.Equal(Validator.PageSizeBetweenRange, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Paging")]
    public void Validate_PageSizeIsValid_ShouldNotHaveValidationError()
    {
        var request = new Request(null, null, null, null, PageSize: 10);

        TestValidationResult<Request> result = _validator.TestValidate(request);

        Assert.True(result.IsValid);
    }
    #endregion

    #region Status Tests
    [Fact]
    [Trait("UT", "Status")]
    public void Validate_StatusIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var invalidStatus = (ProductStatus)999;

        var request = new Request(
            CategoryId: null,
            SearchName: null,
            MinPrice: null,
            MaxPrice: null,
            Status: invalidStatus
        );

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.Status), error.PropertyName);
        Assert.Equal(Validator.InvalidProductStatus, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Status")]
    public void Validate_StatusIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(
            CategoryId: null,
            SearchName: null,
            MinPrice: null,
            MaxPrice: null,
            Status: ProductStatus.InStock
        );

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    #endregion

    #region Passed All Tests
    [Fact]
    [Trait("UT", "General")]
    public void Validate_RequestIsValid_ShouldNotHaveAnyValidationErrors()
    {
        var request = new Request(null, "Laptop", 10, 100, 0, 10);

        TestValidationResult<Request> result = _validator.TestValidate(request);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    #endregion
}
