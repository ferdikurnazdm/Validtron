using FluentAssertions;
using Validtron.UnitTest.Fixtures;

namespace Validtron.UnitTest.Rules;

public sealed class NestedValidationTests
{
    [Fact]
    public void SetValidator_WhenChildFails_ShouldPrefixChildPropertyPath()
    {
        // Arrange
        var validator = new NestedValidator();

        var person = new Person
        {
            Address = new Address
            {
                City = null
            }
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().ContainSingle();

        var error = result.Errors.Single();

        _ = error.PropertyName.Should().Be("Address.City");

        _ = error.ErrorMessage.Should().Be("city");
    }

    [Fact]
    public void SetValidator_WhenChildIsNull_ShouldSkipChildValidation()
    {
        // Arrange
        var validator = new NestedValidator();

        var person = new Person
        {
            Address = null
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void NotNull_WhenChildIsNull_ShouldReturnRequiredFailure()
    {
        // Arrange
        var validator = new RequiredNestedValidator();

        var person = new Person
        {
            Address = null
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().ContainSingle();

        var error = result.Errors.Single();

        _ = error.PropertyName.Should().Be("Address");

        _ = error.ErrorMessage.Should().Be("address");
    }

    [Fact]
    public void RuleForEach_WhenChildFails_ShouldPrefixCollectionIndex()
    {
        // Arrange
        var validator = new CollectionValidator();

        var person = new Person
        {
            Addresses =
            [
                new Address { City = null },

                new Address { City = "Istanbul" }
            ],
            Tags = []
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().Contain(x => x.PropertyName == "Addresses[0].City");
    }

    [Fact]
    public void RuleForEach_WhenCollectionIsNull_ShouldSkipValidation()
    {
        // Arrange
        var validator = new CollectionValidator();

        var person = new Person
        {
            Addresses = null,

            Tags = null
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void RuleForEach_WhenMultipleElementsAreInvalid_ShouldValidateEveryElement()
    {
        // Arrange
        var validator = new CollectionValidator();

        var person = new Person
        {
            Addresses = [],

            Tags = ["", "ok", " "]
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().Contain(
            x => x.PropertyName == "Tags[0]");

        _ = result.Errors.Should().Contain(
            x => x.PropertyName == "Tags[2]");
    }
}
