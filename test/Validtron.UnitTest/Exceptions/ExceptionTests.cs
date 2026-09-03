using FluentAssertions;
using Validtron.Exceptions;
using Validtron.Extensions;
using Validtron.UnitTest.Fixtures;

namespace Validtron.UnitTest.Exceptions;

public sealed class ExceptionTests
{
    [Fact]
    public void InclusiveBetween_WhenMinimumIsGreaterThanMaximum_ShouldThrowInvalidValidationRangeException()
    {
        // Arrange
        var action = () => new InvalidRangeValidator();

        // Act & Assert
        _ = action.Should().Throw<InvalidValidationRangeException>();
    }

    [Fact]
    public void Length_WhenMinimumIsGreaterThanMaximum_ShouldThrowInvalidValidationLengthRangeException()
    {
        // Arrange
        var action = () => new InvalidLengthValidator();

        // Act & Assert
        _ = action.Should().Throw<InvalidValidationLengthRangeException>();
    }

    private sealed class InvalidRangeValidator : Validator<Person>
    {
        public InvalidRangeValidator() => _ = RuleFor(x => x.Age).InclusiveBetween(10, 1);
    }

    private sealed class InvalidLengthValidator : Validator<Person>
    {
        public InvalidLengthValidator() => _ = RuleFor(x => x.Name).Length(10, 1);
    }
}
