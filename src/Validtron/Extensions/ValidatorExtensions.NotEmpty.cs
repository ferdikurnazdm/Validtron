using System.Collections;
using Validtron.Builders;
using Validtron.Configurations;

namespace Validtron.Extensions;

public static partial class ValidatorExtensions
{
    public static IRuleBuilder<T, TProperty> NotEmpty<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        string? errorMessage = null)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value => !IsEmpty(value),
            errorMessage ?? ValidationMessages.Get("NotEmpty"));
    }

    private static bool IsEmpty<TProperty>(
        TProperty value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        if (value is IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();

            try
            {
                return !enumerator.MoveNext();
            }
            finally
            {
                (enumerator as IDisposable)?.Dispose();
            }
        }

        return EqualityComparer<TProperty>.Default.Equals(
            value,
            default!);
    }
}
