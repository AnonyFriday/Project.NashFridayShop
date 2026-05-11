using FluentValidation.TestHelper;
using NashFridayStore.API.Features.Customer.Orders.HandleStripeWebhook;

namespace NashFridayStore.UnitTests.Features.Customer.Orders.HandleStripeWebhook;

public class HandleStripeWebhookValidatorTests
{
    private readonly Validator _validator;

    public HandleStripeWebhookValidatorTests()
    {
        _validator = new Validator();
    }

    [Fact]
    [Trait("UT", "HandleStripeWebhook")]
    public void Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request("{ \"id\": \"evt_123\" }", "t=123,v1=abc");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("UT", "JsonPayload")]
    public void Validate_JsonPayloadIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request("", "t=123,v1=abc");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.JsonPayload)
            .WithErrorMessage(Validator.JsonPayloadRequired);
    }

    [Fact]
    [Trait("UT", "StripeSignature")]
    public void Validate_StripeSignatureIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request("{ \"id\": \"evt_123\" }", "");

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StripeSignature)
            .WithErrorMessage(Validator.StripeSignatureRequired);
    }
}
