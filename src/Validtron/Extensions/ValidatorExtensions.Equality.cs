using Validtron.Builders;
using Validtron.Configurations;

namespace Validtron.Extensions;

public static partial class ValidatorExtensions
{
    public static IRuleBuilder<T, TProperty> Equal<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        TProperty expected,
        string? errorMessage = null)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value =>
                EqualityComparer<TProperty>.Default.Equals(
                    value,
                    expected),
            errorMessage ??
            ValidationMessages.Get(
                "Equal",
                expected!));
    }

    public static IRuleBuilder<T, TProperty> NotEqual<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        TProperty unexpected,
        string? errorMessage = null)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value =>
                !EqualityComparer<TProperty>.Default.Equals(
                    value,
                    unexpected),
            errorMessage ??
            ValidationMessages.Get(
                "NotEqual",
                unexpected!));
    }
}
