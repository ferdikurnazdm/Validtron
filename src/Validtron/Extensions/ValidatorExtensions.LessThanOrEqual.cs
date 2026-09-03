using Validtron.Builders;
using Validtron.Configurations;

namespace Validtron.Extensions;

public static partial class ValidatorExtensions
{
    public static IRuleBuilder<T, TProperty> LessThanOrEqual<T, TProperty>(
    this IRuleBuilder<T, TProperty> ruleBuilder,
    TProperty maximum,
    string? errorMessage = null)
    where TProperty : IComparable<TProperty>
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value => value is not null && value.CompareTo(maximum) <= 0,
            errorMessage ??
            ValidationMessages.Get("LessThanOrEqual", maximum!));
    }

    public static IRuleBuilder<T, TProperty?> LessThanOrEqual<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder,
        TProperty maximum,
        string? errorMessage = null)
        where TProperty : struct, IComparable<TProperty>
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value =>
                !value.HasValue ||
                value.Value.CompareTo(maximum) <= 0,
            errorMessage ??
            ValidationMessages.Get("LessThanOrEqual", maximum));
    }
}
