using FluentValidation.TestHelper;
using NashFridayStore.API.Features.Customer.Orders.CreateCheckout;

namespace NashFridayStore.UnitTests.Features.Customer.Orders.CreateCheckout;

public class CreateCheckoutValidatorTests
{
    private readonly Validator _validator;

    public CreateCheckoutValidatorTests()
    {
        _validator = new Validator();
    }

    [Fact]
    [Trait("UT", "CreateCheckout")]
    public void Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request("John Doe", "john.doe@example.com", "123 Main St", "0123456789");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    #region CustomerName Tests
    [Fact]
    [Trait("UT", "CustomerName")]
    public void Validate_CustomerNameIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request("", "john.doe@example.com", "123 Main St", "0123456789");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerName)
            .WithErrorMessage(Validator.CustomerNameRequired);
    }

    [Fact]
    [Trait("UT", "CustomerName")]
    public void Validate_CustomerNameExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(new string('a', 101), "john.doe@example.com", "123 Main St", "0123456789");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerName)
            .WithErrorMessage(Validator.CustomerNameMaxLength);
    }
    #endregion

    #region CustomerEmail Tests
    [Fact]
    [Trait("UT", "CustomerEmail")]
    public void Validate_CustomerEmailIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request("John Doe", "", "123 Main St", "0123456789");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerEmail)
            .WithErrorMessage(Validator.CustomerEmailRequired);
    }

    [Fact]
    [Trait("UT", "CustomerEmail")]
    public void Validate_CustomerEmailIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request("John Doe", "invalid-email", "123 Main St", "0123456789");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerEmail)
            .WithErrorMessage(Validator.CustomerEmailInvalid);
    }

    [Fact]
    [Trait("UT", "CustomerEmail")]
    public void Validate_CustomerEmailExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request("John Doe", new string('a', 90) + "@example.com", "123 Main St", "0123456789");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerEmail)
            .WithErrorMessage(Validator.CustomerEmailMaxLength);
    }
    #endregion

    #region DeliveryAddress Tests
    [Fact]
    [Trait("UT", "DeliveryAddress")]
    public void Validate_DeliveryAddressIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request("John Doe", "john.doe@example.com", "", "0123456789");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DeliveryAddress)
            .WithErrorMessage(Validator.DeliveryAddressRequired);
    }

    [Fact]
    [Trait("UT", "DeliveryAddress")]
    public void Validate_DeliveryAddressExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request("John Doe", "john.doe@example.com", new string('a', 301), "0123456789");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DeliveryAddress)
            .WithErrorMessage(Validator.DeliveryAddressMaxLength);
    }
    #endregion

    #region PhoneNumber Tests
    [Fact]
    [Trait("UT", "PhoneNumber")]
    public void Validate_PhoneNumberIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request("John Doe", "john.doe@example.com", "123 Main St", "");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage(Validator.PhoneNumberRequired);
    }

    [Fact]
    [Trait("UT", "PhoneNumber")]
    public void Validate_PhoneNumberExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request("John Doe", "john.doe@example.com", "123 Main St", new string('0', 21));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage(Validator.PhoneNumberMaxLength);
    }
    #endregion
}
