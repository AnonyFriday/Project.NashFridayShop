using FluentValidation.Results;
using FluentValidation.TestHelper;
using NashFridayStore.API.Features.Products.DeleteProduct;

namespace NashFridayStore.UnitTests.Features.Products;

public class DeleteProductValidatorTests
{
    private readonly Validator _validator;

    public DeleteProductValidatorTests()
    {
        _validator = new Validator();
    }

    #region Id Tests
    [Fact]
    [Trait("UT", "Id")]
    public void Validate_IdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.Empty);

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.Id), error.PropertyName);
        Assert.Equal(Validator.IdRequired, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Id")]
    public void Validate_IdIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid());

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    #endregion
}
