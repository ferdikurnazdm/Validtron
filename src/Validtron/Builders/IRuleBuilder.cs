using Validtron.Configurations;

namespace Validtron.Builders;

public interface IRuleBuilderInitial<T, TProperty> : IRuleBuilder<T, TProperty> { }

public interface IRuleBuilder<T, TProperty>
{
    IRuleBuilder<T, TProperty> Must(Func<TProperty, bool> predicate, string errorMessage);

    IRuleBuilder<T, TProperty> Must(Func<T, TProperty, bool> predicate, string errorMessage);

    IRuleBuilder<T, TProperty> MustAsync(
        Func<TProperty, CancellationToken, Task<bool>> predicate,
        string errorMessage);

    IRuleBuilder<T, TProperty> MustAsync(
        Func<T, TProperty, CancellationToken, Task<bool>> predicate,
        string errorMessage);

    IRuleBuilder<T, TProperty> When(Func<T, bool> condition);

    IRuleBuilder<T, TProperty> Unless(Func<T, bool> condition);

    IRuleBuilder<T, TProperty> Cascade(CascadeMode mode);

    IRuleBuilder<T, TProperty> SetValidator(IValidator<TProperty> validator);
}
