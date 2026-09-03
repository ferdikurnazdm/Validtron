using Validtron.Builders;
using Validtron.Configurations;

namespace Validtron.Extensions;

public static partial class ValidatorExtensions
{
    public static IRuleBuilder<T, TProperty> GreaterThan<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        TProperty minimum,
        string? errorMessage = null)
        where TProperty : IComparable<TProperty>
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value =>
                value is not null &&
                value.CompareTo(minimum) > 0,
            errorMessage ??
            ValidationMessages.Get(
                "GreaterThan",
                minimum!));
    }

    public static IRuleBuilder<T, TProperty?> GreaterThan<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder,
        TProperty minimum,
        string? errorMessage = null)
        where TProperty : struct, IComparable<TProperty>
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value =>
                !value.HasValue ||
                value.Value.CompareTo(minimum) > 0,
            errorMessage ??
            ValidationMessages.Get(
                "GreaterThan",
                minimum));
    }
}
