using FluentValidation.TestHelper;
using NashFridayStore.API.Features.Customer.Cart.CreateOrAddItemToCart;

namespace NashFridayStore.UnitTests.Features.Customer.Cart.CreateOrAddItemToCart;

public class CreateOrAddItemToCartValidatorTests
{
    private readonly Validator _validator;

    public CreateOrAddItemToCartValidatorTests()
    {
        _validator = new Validator();
    }

    [Fact]
    [Trait("UT", "CreateOrAddItemToCart")]
    public void Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            1,
            "Product Name",
            "https://example.com/image.png",
            100.00m,
            Guid.NewGuid(),
            "Category Name");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("UT", "ProductId")]
    public void Validate_ProductIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.Empty,
            1,
            "Product Name",
            "https://example.com/image.png",
            100.00m,
            Guid.NewGuid(),
            "Category Name");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId)
            .WithErrorMessage(Validator.ProductIdRequired);
    }

    [Fact]
    [Trait("UT", "Quantity")]
    public void Validate_QuantityIsZero_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            0,
            "Product Name",
            "https://example.com/image.png",
            100.00m,
            Guid.NewGuid(),
            "Category Name");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Quantity)
            .WithErrorMessage(Validator.QuantityInvalid);
    }

    [Fact]
    [Trait("UT", "ProductName")]
    public void Validate_ProductNameIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            1,
            "",
            "https://example.com/image.png",
            100.00m,
            Guid.NewGuid(),
            "Category Name");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductName)
            .WithErrorMessage(Validator.ProductNameRequired);
    }

    [Fact]
    [Trait("UT", "Price")]
    public void Validate_PriceIsZeroOrNegative_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            1,
            "Product Name",
            "https://example.com/image.png",
            0m,
            Guid.NewGuid(),
            "Category Name");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price)
            .WithErrorMessage(Validator.PriceInvalid);
    }

    [Fact]
    [Trait("UT", "CategoryId")]
    public void Validate_CategoryIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            1,
            "Product Name",
            "https://example.com/image.png",
            100.00m,
            Guid.Empty,
            "Category Name");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId)
            .WithErrorMessage(Validator.CategoryIdRequired);
    }

    [Fact]
    [Trait("UT", "CategoryName")]
    public void Validate_CategoryNameIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(
            Guid.NewGuid(),
            1,
            "Product Name",
            "https://example.com/image.png",
            100.00m,
            Guid.NewGuid(),
            "");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryName)
            .WithErrorMessage(Validator.CategoryNameRequired);
    }
}
