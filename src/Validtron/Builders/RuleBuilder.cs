using Validtron.Configurations;
using Validtron.Rules;

namespace Validtron.Builders;

internal sealed class RuleBuilder<T, TProperty> : IRuleBuilderInitial<T, TProperty>
{
    private readonly IConfigurableValidationRule<T, TProperty> _rule;

    internal RuleBuilder(IConfigurableValidationRule<T, TProperty> rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        _rule = rule;
    }

    public IRuleBuilder<T, TProperty> Must(Func<TProperty, bool> predicate, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

        _rule.AddStep(ValidationStep<T, TProperty>
            .Sync((_, value) => predicate(value), errorMessage));

        return this;
    }

    public IRuleBuilder<T, TProperty> Must(Func<T, TProperty, bool> predicate, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

        _rule.AddStep(ValidationStep<T, TProperty>.Sync(predicate, errorMessage));

        return this;
    }

    public IRuleBuilder<T, TProperty> MustAsync(Func<TProperty, CancellationToken, Task<bool>> predicate, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

        _rule.AddStep(ValidationStep<T, TProperty>.Async(
            (_, value, cancellationToken) =>
                predicate(value, cancellationToken), errorMessage));

        return this;
    }

    public IRuleBuilder<T, TProperty> MustAsync(Func<T, TProperty, CancellationToken, Task<bool>> predicate, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

        _rule.AddStep(ValidationStep<T, TProperty>.Async(predicate, errorMessage));

        return this;
    }

    public IRuleBuilder<T, TProperty> When(Func<T, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);

        _rule.AddCondition(condition);

        return this;
    }

    public IRuleBuilder<T, TProperty> Unless(Func<T, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);

        _rule.AddCondition(instance => !condition(instance));

        return this;
    }

    public IRuleBuilder<T, TProperty> Cascade(CascadeMode mode)
    {
        _rule.SetCascadeMode(mode);

        return this;
    }

    public IRuleBuilder<T, TProperty> SetValidator(IValidator<TProperty> validator)
    {
        ArgumentNullException.ThrowIfNull(validator);

        _rule.AddStep(ValidationStep<T, TProperty>.Child(validator));

        return this;
    }
}
