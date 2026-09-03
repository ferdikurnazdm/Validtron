using FluentAssertions;
using Validtron.Exceptions;
using Validtron.UnitTest.Fixtures;

namespace Validtron.UnitTest.Async;

public sealed class AsyncValidationTests
{
    [Fact]
    public void Validate_WhenValidatorContainsAsyncRule_ShouldThrowAsyncValidationRequiredException()
    {
        // Arrange
        var validator = new AsyncUserNameValidator();

        var request = new RegistrationRequest
        {
            UserName = "taken"
        };

        // Act
        var action = () => validator.Validate(request);

        // Assert
        _ = action.Should()
            .Throw<AsyncValidationRequiredException>()
            .WithMessage("*ValidateAsync*");
    }

    [Fact]
    public async Task ValidateAsync_WhenAsyncRuleFails_ShouldReturnFailure()
    {
        // Arrange
        var validator = new AsyncUserNameValidator();

        var request = new RegistrationRequest
        {
            UserName = "taken"
        };

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        _ = result.Errors.Should().ContainSingle();

        var error = result.Errors.Single();

        _ = error.PropertyName.Should().Be("UserName");
        _ = error.ErrorMessage.Should().Be("taken");
    }

    [Fact]
    public async Task ValidateAsync_WhenAsyncRulePasses_ShouldReturnValidResult()
    {
        // Arrange
        var validator = new AsyncUserNameValidator();

        var request = new RegistrationRequest
        {
            UserName = "available"
        };

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        _ = result.IsValid.Should().BeTrue();

        _ = result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenChildValidatorContainsAsyncRule_ShouldThrowAsyncValidationRequiredException()
    {
        // Arrange
        var validator = new AsyncChildParentValidator();

        var person = new Person
        {
            Address = new Address
            {
                City = "blocked"
            }
        };

        // Act
        var action = () => validator.Validate(person);

        // Assert
        _ = action.Should()
            .Throw<AsyncValidationRequiredException>();
    }

    [Fact]
    public async Task ValidateAsync_WhenAsyncChildFails_ShouldPrefixChildPropertyPath()
    {
        // Arrange
        var validator = new AsyncChildParentValidator();

        var person = new Person
        {
            Address = new Address
            {
                City = "blocked"
            }
        };

        // Act
        var result = await validator.ValidateAsync(person);

        // Assert
        _ = result.Errors.Should().ContainSingle();

        _ = result.Errors.Single().PropertyName.Should().Be("Address.City");

        _ = result.Errors.Single().ErrorMessage.Should().Be("blocked");
    }

    [Fact]
    public async Task ValidateAsync_WhenCancellationIsRequested_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var validator = new AsyncUserNameValidator();

        var request = new RegistrationRequest
        {
            UserName = "taken"
        };

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act
        var action = () =>
            validator.ValidateAsync(
                request,
                cancellationTokenSource.Token);

        // Assert
        _ = await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }
}
