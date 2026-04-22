#pragma warning disable IDE0008

using FluentValidation.TestHelper;
using NashFridayStore.SharedFeatures.Features.Categories.GetCategories;

namespace NashFridayStore.UnitTests.Features.Categories;

public class GetCategoriesValidatorTests
{
    private readonly Validator _validator;

    public GetCategoriesValidatorTests()
    {
        _validator = new Validator();
    }

    #region SearchName Tests
    [Fact]
    [Trait("UT", "SearchName")]
    public void Validate_SearchNameIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(SearchName: "Electronics");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    [Trait("UT", "SearchName")]
    public void Validate_SearchNameExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var searchName = new string('a', 101);
        var request = new Request(SearchName: searchName);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.SearchName), error.PropertyName);
        Assert.Equal(Validator.SearchNameMaxLength, error.ErrorMessage);
    }
    #endregion

    #region PageIndex Tests
    [Fact]
    [Trait("UT", "PageIndex")]
    public void Validate_PageIndexIsNegative_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(null, PageIndex: -1);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.PageIndex), error.PropertyName);
        Assert.Equal(Validator.PageIndexGreaterThanOrEqualZero, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "PageIndex")]
    public void Validate_PageIndexIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(null, PageIndex: 1);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    #endregion

    #region PageSize Tests
    [Fact]
    [Trait("UT", "PageSize")]
    public void Validate_PageSizeIsZero_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(null, PageSize: 0);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.PageSize), error.PropertyName);
        Assert.Equal(Validator.PageSizeBetweenRange, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "PageSize")]
    public void Validate_PageSizeExceedsMax_ShouldHaveValidationError()
    {
        // Arrange
        var request = new Request(null, PageSize: 101);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(nameof(Request.PageSize), error.PropertyName);
        Assert.Equal(Validator.PageSizeBetweenRange, error.ErrorMessage);
    }

    [Fact]
    [Trait("UT", "PageSize")]
    public void Validate_PageSizeIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new Request(null, PageSize: 50);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    #endregion

    #region IsAll Tests
    [Fact]
    [Trait("UT", "IsAll")]
    public void Validate_IsAllTrueWithInvalidPageIndex_ShouldBeValid()
    {
        // Arrange
        var request = new Request(null, PageIndex: -1, IsAll: true);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    [Trait("UT", "IsAll")]
    public void Validate_IsAllTrueWithInvalidPageSize_ShouldBeValid()
    {
        // Arrange
        var request = new Request(null, PageSize: 0, IsAll: true);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    [Trait("UT", "IsAll")]
    public void Validate_IsAllFalseWithValidRequest_ShouldBeValid()
    {
        // Arrange
        var request = new Request(null, PageIndex: 0, PageSize: 10, IsAll: false);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    #endregion
}
