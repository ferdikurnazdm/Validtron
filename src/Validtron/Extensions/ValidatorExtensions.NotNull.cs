using Validtron.Builders;
using Validtron.Configurations;

namespace Validtron.Extensions;

public static partial class ValidatorExtensions
{
    public static IRuleBuilder<T, TProperty> NotNull<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        string? errorMessage = null)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value => value is not null,
            errorMessage ?? ValidationMessages.Get("NotNull"));
    }
}
