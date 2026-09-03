using System.Text.RegularExpressions;
using Validtron.Builders;
using Validtron.Configurations;

namespace Validtron.Extensions;

public static partial class ValidatorExtensions
{
    public static IRuleBuilder<T, TProperty> Matches<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        string pattern,
        string? errorMessage = null)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

        var regex = new Regex(
            pattern,
            RegexOptions.CultureInvariant,
            TimeSpan.FromSeconds(1));

        return ruleBuilder.Must(
            value =>
                value is null ||
                (value is string text &&
                 regex.IsMatch(text)),
            errorMessage ?? ValidationMessages.Get("Matches"));
    }
}
