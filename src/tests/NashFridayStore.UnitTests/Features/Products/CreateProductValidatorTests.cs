using FluentValidation.Results;
using FluentValidation.TestHelper;
using NashFridayStore.API.Features.Products.CreateProduct;
using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.UnitTests.Features.Products;

public class CreateProductValidatorTests
{
    private readonly Validator _validator;

    public CreateProductValidatorTests()
    {
        _validator = new Validator();
    }

    #region CategoryId Tests
    [Fact]
    [Trait("UT", "CategoryId")]
    public void Validate_CategoryIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.Empty,
            "Product Name",
            "Description",
            10.5m,
            5);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.False(result.IsValid);
        Assert.Equal(nameof(Request.CategoryId), error.PropertyName);
        Assert.Equal(Validator.CategoryIdRequired, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "CategoryId")]
    public void Validate_CategoryIdIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            "Product Name",
            "Description",
            10.5m,
            5);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region Name Tests
    [Fact]
    [Trait("UT", "Name")]
    public void Validate_NameIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            string.Empty,
            "Description",
            10.5m,
            5);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(Validator.NameRequired, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Name")]
    public void Validate_NameExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        string longName = new string('a', 256);
        var request = new Request(
            Guid.NewGuid(),
            longName,
            "Description",
            10.5m,
            5);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(Validator.NameMaxLength, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Name")]
    public void Validate_NameIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            "Valid Product Name",
            "Description",
            10.5m, 5);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region Description Tests
    [Fact]
    [Trait("UT", "Description")]
    public void Validate_DescriptionIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            "Product Name",
            string.Empty,
            10.5m, 5);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(Validator.DescriptionRequired, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Description")]
    public void Validate_DescriptionExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        string longDescription = new string('a', 1001);
        var request = new Request(
            Guid.NewGuid(),
            "Product Name",
            longDescription,
            10.5m,
            5);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(Validator.DescriptionMaxLength, error.ErrorMessage);
    }
    #endregion

    #region Price Tests
    [Fact]
    [Trait("UT", "Price")]
    public void Validate_PriceIsZero_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            "Product Name",
            "Description",
            0,
            5);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(Validator.PriceGreaterThanZero, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Price")]
    public void Validate_PriceIsNegative_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            "Product Name",
            "Description",
            -10.5m,
            5);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    [Trait("UT", "Price")]
    public void Validate_PriceIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            "Product Name",
            "Description",
            99.99m,
            5);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion



    #region Quantity Tests
    [Fact]
    [Trait("UT", "Quantity")]
    public void Validate_QuantityIsNegative_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            "Product Name",
            "Description",
            10.5m,
            -1);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(Validator.QuantityGreaterThanOrEqualZero, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Quantity")]
    public void Validate_QuantityIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            "Product Name",
            "Description",
            10.5m,
            0);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region Status Tests
    [Fact]
    [Trait("UT", "Status")]
    public void Validate_StatusIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            "Product Name",
            "Description",
            10.5m,
            5,
            ProductStatus.InStock);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region General Tests
    [Fact]
    [Trait("UT", "General")]
    public void Validate_AllFieldsValid_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            "Valid Product Name",
            "Valid Description",
            99.99m,
            10,
            ProductStatus.InStock);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    #endregion
}
