using FluentValidation.Results;
using FluentValidation.TestHelper;
using NashFridayStore.API.Features.Products.PostProductRating;

namespace NashFridayStore.UnitTests.Features.Products;

public class PostProductRatingValidatorTests
{
    private readonly Validator _validator;

    public PostProductRatingValidatorTests()
    {
        _validator = new Validator();
    }

    #region ProductId Tests
    [Fact]
    [Trait("UT", "ProductId")]
    public void Validate_ProductIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.Empty, new RequestBody("Comment", 5));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.ProductId), error.PropertyName);
        Assert.Equal(Validator.ProductIdIsRequired, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "ProductId")]
    public void Validate_ProductIdIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), new RequestBody("Comment", 5));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region Stars Tests
    [Theory]
    [Trait("UT", "Stars")]
    [InlineData(0)]
    [InlineData(11)]
    public void Validate_StarsOutOfRange_ShouldHaveValidationError(int stars)
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), new RequestBody("Comment", stars));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal("Stars", error.PropertyName);
        Assert.Equal(Validator.StarsInRange, error.ErrorMessage);
    }

    [Theory]
    [Trait("UT", "Stars")]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Validate_StarsInRange_ShouldNotHaveValidationError(int stars)
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), new RequestBody("Comment", stars));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion

    #region Comment Tests
    [Fact]
    [Trait("UT", "Comment")]
    public void Validate_CommentExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        string comment = new string('a', 1001);
        var request = new Request(Guid.NewGuid(), new RequestBody(comment, 5));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);

        ValidationFailure error = Assert.Single(result.Errors);
        Assert.Equal("Comment", error.PropertyName);
        Assert.Equal(Validator.CommentNotExceedLength, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "Comment")]
    public void Validate_CommentWithinMaxLength_ShouldNotHaveValidationError()
    {
        // Arrange
        string comment = new string('a', 1000);
        var request = new Request(Guid.NewGuid(), new RequestBody(comment, 5));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    [Trait("UT", "Comment")]
    public void Validate_CommentIsNull_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), new RequestBody(null, 5));

        // Act
        TestValidationResult<Request> result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
    }
    #endregion
}
