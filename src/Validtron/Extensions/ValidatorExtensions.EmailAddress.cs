using System.Text.RegularExpressions;
using Validtron.Builders;
using Validtron.Configurations;

namespace Validtron.Extensions;

public static partial class ValidatorExtensions
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.CultureInvariant,
        TimeSpan.FromSeconds(1));

    public static IRuleBuilder<T, TProperty> EmailAddress<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        string? errorMessage = null)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value =>
                value is null ||
                (value is string text &&
                 EmailRegex.IsMatch(text)),
            errorMessage ??
            ValidationMessages.Get("EmailAddress"));
    }
}
