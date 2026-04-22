using FluentValidation.Results;
using FluentValidation.TestHelper;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.SharedFeatures.Features.Products.UpdateProduct;

namespace NashFridayStore.UnitTests.Features.Products;

public class UpdateProductValidatorTests
{
    private readonly Validator _validator;

    public UpdateProductValidatorTests()
    {
        _validator = new Validator();
    }

    #region Id Tests
    [Fact]
    [Trait("UT", "Id")]
    public void Validate_IdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.Empty,
            new RequestBody(
                Guid.NewGuid(),
                "Product Name",
                "Description",
                10.5m,
                "https://image.url",
                5,
                ProductStatus.InStock));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(Validator.IdRequired, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Id")]
    public void Validate_IdIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            new RequestBody(

            Guid.NewGuid(),
            "Product Name",
            "Description",
            10.5m,
            "https://image.url",
            5,
            ProductStatus.InStock));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region CategoryId Tests
    [Fact]
    [Trait("UT", "CategoryId")]
    public void Validate_CategoryIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            new RequestBody(
                Guid.Empty,
                "Product Name",
                "Description",
                10.5m,
                "https://image.url",
                5,
                ProductStatus.InStock));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(Validator.CategoryIdRequired, error.ErrorMessage);
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
            new RequestBody(
                Guid.NewGuid(),
                string.Empty,
                "Description",
                10.5m,
                "https://image.url",
                5,
                ProductStatus.InStock));

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
            new RequestBody(
                Guid.NewGuid(),
                longName,
                "Description",
                10.5m,
                "https://image.url",
                5,
                ProductStatus.InStock));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(Validator.NameMaxLength, error.ErrorMessage);
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
            new RequestBody(
                Guid.NewGuid(),
                "Product Name",
                string.Empty,
                10.5m,
                "https://image.url",
                5,
                ProductStatus.InStock));

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
            new RequestBody(
                Guid.NewGuid(),
                "Product Name",
                longDescription,
                10.5m,
                "https://image.url",
                5,
                ProductStatus.InStock));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
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
            new RequestBody(
                Guid.NewGuid(),
                "Product Name",
                "Description",
                0,
                "https://image.url",
                5,
                ProductStatus.InStock));

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
            new RequestBody(
                Guid.NewGuid(),
                "Product Name",
                "Description",
                99.99m,
                "https://image.url",
                5,
                ProductStatus.InStock));

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
            new RequestBody(
                Guid.NewGuid(),
                "Valid Product Name",
                "Valid Description",
                99.99m,
                "https://image.url",
                10,
                ProductStatus.InStock));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    #endregion
}
