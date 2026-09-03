using Validtron.Builders;
using Validtron.Configurations;
using Validtron.Exceptions;

namespace Validtron.Extensions;

public static partial class ValidatorExtensions
{
    public static IRuleBuilder<T, TProperty> InclusiveBetween<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        TProperty minimum,
        TProperty maximum,
        string? errorMessage = null)
        where TProperty : IComparable<TProperty>
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);
        EnsureValidRange(minimum, maximum);

        return ruleBuilder.Must(
            value =>
                value is not null &&
                value.CompareTo(minimum) >= 0 &&
                value.CompareTo(maximum) <= 0,
            errorMessage ??
            ValidationMessages.Get(
                "InclusiveBetween",
                minimum!,
                maximum!));
    }

    public static IRuleBuilder<T, TProperty?> InclusiveBetween<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder,
        TProperty minimum,
        TProperty maximum,
        string? errorMessage = null)
        where TProperty : struct, IComparable<TProperty>
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);
        EnsureValidRange(minimum, maximum);

        return ruleBuilder.Must(
            value =>
                !value.HasValue ||
                (value.Value.CompareTo(minimum) >= 0 &&
                 value.Value.CompareTo(maximum) <= 0),
            errorMessage ??
            ValidationMessages.Get(
                "InclusiveBetween",
                minimum,
                maximum));
    }

    private static void EnsureValidRange<TProperty>(
        TProperty minimum,
        TProperty maximum)
        where TProperty : IComparable<TProperty>
    {
        if (minimum.CompareTo(maximum) > 0)
        {
            throw new InvalidValidationRangeException();
        }
    }
}
