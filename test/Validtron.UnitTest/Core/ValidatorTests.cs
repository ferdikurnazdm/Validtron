using FluentAssertions;
using Validtron.UnitTest.Fixtures;

namespace Validtron.UnitTest.Core;

public sealed class ValidatorTests
{
    [Fact]
    public void Validate_WhenInstanceIsNull_ShouldReturnRootFailure()
    {
        // Arrange
        var validator = new NameValidator();

        // Act
        var result = validator.Validate(null!);

        // Assert
        _ = result.IsValid.Should().BeFalse();

        _ = result.Errors.Should().ContainSingle();

        var error = result.Errors.Single();

        _ = error.PropertyName.Should().BeEmpty();

        _ = error.ErrorMessage.Should().Be("The object to validate cannot be null.");
    }

    [Fact]
    public void Validate_WhenAllRulesPass_ShouldReturnValidResult()
    {
        // Arrange
        var validator = new NameValidator();

        var person = new Person
        {
            Name = "Ferdi"
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.IsValid.Should().BeTrue();

        _ = result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Cascade_WhenModeIsContinue_ShouldReturnAllFailures()
    {
        // Arrange
        var validator = new ContinueNameValidator();

        var person = new Person
        {
            Name = string.Empty
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().HaveCount(2);

        _ = result.Errors.Should().Contain(x => x.ErrorMessage == "required");

        _ = result.Errors.Should().Contain(x => x.ErrorMessage == "min");
    }

    [Fact]
    public void Cascade_WhenModeIsStop_ShouldStopAfterFirstFailure()
    {
        // Arrange
        var validator = new StopNameValidator();

        var person = new Person
        {
            Name = string.Empty
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().ContainSingle();

        _ = result.Errors.Single().ErrorMessage.Should().Be("required");
    }

    [Fact]
    public void ValidationResult_WhenSamePropertyAndMessageAddedTwice_ShouldDeduplicateFailure()
    {
        // Arrange
        var validator = new DuplicateFailureValidator();

        var person = new Person
        {
            Name = "x"
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().ContainSingle();

        _ = result.Errors.Single().PropertyName.Should().Be("Name");

        _ = result.Errors.Single().ErrorMessage.Should().Be("same");
    }

    [Fact]
    public void ErrorsByProperty_WhenPropertyHasMultipleFailures_ShouldGroupMessages()
    {
        // Arrange
        var validator = new ContinueNameValidator();

        var person = new Person
        {
            Name = string.Empty
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.ErrorsByProperty.Should().ContainKey("Name");

        _ = result.ErrorsByProperty["Name"].Should().HaveCount(2);

        _ = result.ErrorsByProperty["Name"].Should().Contain("required");

        _ = result.ErrorsByProperty["Name"].Should().Contain("min");
    }
}
