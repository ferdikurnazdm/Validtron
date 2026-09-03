using Validtron.Builders;
using Validtron.Configurations;
using Validtron.Exceptions;

namespace Validtron.Extensions;

public static partial class ValidatorExtensions
{
    public static IRuleBuilder<T, TProperty> MinimumLength<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        int minimumLength,
        string? errorMessage = null)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);
        ArgumentOutOfRangeException.ThrowIfNegative(minimumLength);

        return ruleBuilder.Must(
            value =>
                value is null ||
                (value is string text &&
                 text.Length >= minimumLength),
            errorMessage ??
            ValidationMessages.Get(
                "MinimumLength",
                minimumLength));
    }

    public static IRuleBuilder<T, TProperty> MaximumLength<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        int maximumLength,
        string? errorMessage = null)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumLength);

        return ruleBuilder.Must(
            value =>
                value is null ||
                (value is string text &&
                 text.Length <= maximumLength),
            errorMessage ??
            ValidationMessages.Get(
                "MaximumLength",
                maximumLength));
    }

    public static IRuleBuilder<T, TProperty> Length<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        int minimumLength,
        int maximumLength,
        string? errorMessage = null)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);
        ArgumentOutOfRangeException.ThrowIfNegative(minimumLength);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumLength);

        if (minimumLength > maximumLength)
        {
            throw new InvalidValidationLengthRangeException();
        }

        return ruleBuilder.Must(
            value =>
                value is null ||
                (value is string text &&
                 text.Length >= minimumLength &&
                 text.Length <= maximumLength),
            errorMessage ??
            ValidationMessages.Get(
                "Length",
                minimumLength,
                maximumLength));
    }
}