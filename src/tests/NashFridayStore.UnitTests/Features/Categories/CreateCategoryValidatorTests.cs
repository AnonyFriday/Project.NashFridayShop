using FluentValidation.Results;
using NashFridayStore.API.Features.Categories.CreateCategory;

namespace NashFridayStore.UnitTests.Features.Categories;

public class CreateCategoryValidatorTests
{
    private readonly Validator _validator = new();

    [Fact]
    [Trait("UT", "CreateCategory")]
    public void Validator_ValidRequest_ShouldNotHaveErrors()
    {
        // Arrange
        var request = new Request("Electronics", "Category for electronics");

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [Trait("UT", "CreateCategory")]
    public void Validator_EmptyName_ShouldHaveError(string? name)
    {
        // Arrange
        var request = new Request(name!, "Description");

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(Request.Name));
    }

    [Fact]
    [Trait("UT", "CreateCategory")]
    public void Validator_NameTooLong_ShouldHaveError()
    {
        // Arrange
        var request = new Request(new string('a', 101), "Description");

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(Request.Name));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [Trait("UT", "CreateCategory")]
    public void Validator_EmptyDescription_ShouldHaveError(string? description)
    {
        // Arrange
        var request = new Request("Name", description!);

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(Request.Description));
    }

    [Fact]
    [Trait("UT", "CreateCategory")]
    public void Validator_DescriptionTooLong_ShouldHaveError()
    {
        // Arrange
        var request = new Request("Name", new string('a', 301));

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(Request.Description));
    }
}
