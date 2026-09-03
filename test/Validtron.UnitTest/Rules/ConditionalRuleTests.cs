using FluentAssertions;
using Validtron.UnitTest.Fixtures;

namespace Validtron.UnitTest.Rules;

public sealed class ConditionalRuleTests
{
    [Fact]
    public void When_WhenConditionIsTrue_ShouldExecuteRule()
    {
        // Arrange
        var validator = new ConditionalValidator();

        var person = new Person
        {
            IsCompany = true,

            TaxNumber = null
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().Contain(
            x => x.PropertyName == "TaxNumber" &&
                 x.ErrorMessage == "tax");
    }

    [Fact]
    public void When_WhenConditionIsFalse_ShouldSkipRule()
    {
        // Arrange
        var validator = new ConditionalValidator();

        var person = new Person
        {
            IsCompany = false,

            TaxNumber = null,

            ReferralCode = "REF-1"
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().NotContain(
            x => x.PropertyName == "TaxNumber");
    }

    [Fact]
    public void Unless_WhenConditionIsFalse_ShouldExecuteRule()
    {
        // Arrange
        var validator = new ConditionalValidator();

        var person = new Person
        {
            IsCompany = false,

            ReferralCode = null
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        _ = result.Errors.Should().Contain(
            x => x.PropertyName == "ReferralCode" &&
                 x.ErrorMessage == "referral");
    }

    [Theory]
    [InlineData(true, 18, true)]
    [InlineData(true, 17, false)]
    [InlineData(false, 18, false)]
    [InlineData(false, 17, false)]
    public void When_WhenMultipleConditionsAreConfigured_ShouldCombineWithAnd(
        bool isCompany,
        int age,
        bool shouldExecute)
    {
        // Arrange
        var validator = new MultipleConditionValidator();

        var person = new Person
        {
            IsCompany = isCompany,

            Age = age,

            TaxNumber = null
        };

        // Act
        var result = validator.Validate(person);

        // Assert
        var hasTaxError = result.Errors.Any(x => x.PropertyName == "TaxNumber");

        _ = hasTaxError.Should().Be(shouldExecute);
    }
}
