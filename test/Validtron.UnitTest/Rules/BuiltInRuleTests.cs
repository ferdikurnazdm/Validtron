using FluentAssertions;
using Validtron.UnitTest.Fixtures;

namespace Validtron.UnitTest.Rules;

public sealed class BuiltInRuleTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("A", true)]
    public void NotEmpty_WhenValueChanges_ShouldReturnExpectedValidationResult(
        string? value,
        bool expectedIsValid)
    {
        // Arrange
        var validator = new NameValidator();

        var person = new Person
        {
            Name = value
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void StringRules_WhenAllValuesAreValid_ShouldReturnValidResult()
    {
        // Arrange
        var validator = new StringRulesValidator();

        var person = new Person
        {
            Name = "ABC",
            Email = "test@example.com"
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.IsValid.Should().BeTrue();

        _ = result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("A", "min")]
    [InlineData("ABCDEF", "max")]
    [InlineData("Ab", "matches")]
    public void StringRules_WhenValueIsInvalid_ShouldReturnExpectedFailure(
        string name,
        string expectedMessage)
    {
        // Arrange
        var validator = new StringRulesValidator();

        var person = new Person
        {
            Name = name,
            Email = "test@example.com"
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().Contain(
            x => x.ErrorMessage == expectedMessage);
    }

    [Fact]
    public void EmailAddress_WhenEmailIsInvalid_ShouldReturnFailure()
    {
        // Arrange
        var validator = new StringRulesValidator();

        var person = new Person
        {
            Name = "ABC",
            Email = "invalid"
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().Contain(
            x => x.PropertyName == "Email" &&
                 x.ErrorMessage == "email");
    }

    [Fact]
    public void EqualityRules_WhenValueMatchesContract_ShouldReturnValidResult()
    {
        // Arrange
        var validator = new EqualityValidator();

        var person = new Person
        {
            Name = "Ferdi"
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Equal_WhenValueDoesNotMatchExpectedValue_ShouldReturnFailure()
    {
        // Arrange
        var validator = new EqualityValidator();

        var person = new Person
        {
            Name = "Other"
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().Contain(
            x => x.ErrorMessage == "equal");
    }

    [Theory]
    [InlineData(11, true)]
    [InlineData(10, false)]
    [InlineData(19, true)]
    [InlineData(20, false)]
    public void ComparisonRules_WhenValueIsOnBoundary_ShouldReturnExpectedValidationResult(
        int age,
        bool expectedIsValid)
    {
        // Arrange
        var validator = new ComparisonValidator();

        var person = new Person
        {
            Age = age,
            Score = null
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void NullableComparison_WhenValueIsNull_ShouldSkipComparison()
    {
        // Arrange
        var validator = new ComparisonValidator();

        var person = new Person
        {
            Age = 15,
            Score = null
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().NotContain(
            x => x.PropertyName == "Score");
    }

    [Fact]
    public void IsInEnum_WhenValueIsUndefined_ShouldReturnFailure()
    {
        // Arrange
        var validator = new EnumValidator();

        var person = new Person
        {
            Role = (UserRole)999
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().ContainSingle();

        _ = result.Errors.Single().ErrorMessage.Should().Be("enum");
    }
}
