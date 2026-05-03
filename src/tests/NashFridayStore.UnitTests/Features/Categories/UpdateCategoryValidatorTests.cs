using FluentValidation.Results;
using NashFridayStore.API.Features.Categories.UpdateCategory;

namespace NashFridayStore.UnitTests.Features.Categories;

public class UpdateCategoryValidatorTests
{
    private readonly Validator _validator = new();

    [Fact]
    [Trait("UT", "UpdateCategory")]
    public void Validator_ValidRequest_ShouldNotHaveErrors()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), new RequestBody("Electronics", "Updated description"));

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    [Trait("UT", "UpdateCategory")]
    public void Validator_EmptyId_ShouldHaveError()
    {
        // Arrange
        var request = new Request(Guid.Empty, new RequestBody("Name", "Description"));

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(Request.Id));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [Trait("UT", "UpdateCategory")]
    public void Validator_EmptyName_ShouldHaveError(string? name)
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), new RequestBody(name!, "Description"));

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Body.Name");
    }

    [Fact]
    [Trait("UT", "UpdateCategory")]
    public void Validator_NameTooLong_ShouldHaveError()
    {
        // Arrange
        var request = new Request(Guid.NewGuid(), new RequestBody(new string('a', 101), "Description"));

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Body.Name");
    }
}
