using System.Linq.Expressions;
using Validtron.Builders;
using Validtron.Configurations;
using Validtron.Exceptions;
using Validtron.Internal;
using Validtron.Results;
using Validtron.Rules;

namespace Validtron;

public interface IValidator<in T>
{
    ValidationResult Validate(T instance);

    Task<ValidationResult> ValidateAsync(
        T instance,
        CancellationToken cancellationToken = default);
}

public abstract class Validator<T> : IValidator<T>, IAsyncValidationMetadata
{
    private readonly List<IValidationRule<T>> _rules = [];

    bool IAsyncValidationMetadata.ContainsAsyncValidator =>
        _rules.Any(rule => rule.ContainsAsyncValidator);

    protected IRuleBuilderInitial<T, TProperty> RuleFor<TProperty>(
        Expression<Func<T, TProperty>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var rule = new ValidationRule<T, TProperty>(expression);

        _rules.Add(rule);

        return new RuleBuilder<T, TProperty>(rule);
    }






    protected IRuleBuilderInitial<T, TProperty> RuleForEach<TProperty>(
        Expression<Func<T, IEnumerable<TProperty>?>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var rule = new ValidationRuleCollection<T, TProperty>(expression);

        _rules.Add(rule);

        return new RuleBuilder<T, TProperty>(rule);
    }







    public ValidationResult Validate(T instance)
    {
        var result = new ValidationResult();

        if (instance is null)
        {
            result.AddError(string.Empty, ValidationMessages.Get("NullInstance"));

            return result;
        }

        if (_rules.Any(rule => rule.ContainsAsyncValidator))
        {
            throw new AsyncValidationRequiredException();
        }

        foreach (var rule in _rules)
        {
            rule.Validate(instance, result);
        }

        return result;
    }

    public async Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken = default)
    {
        var result = new ValidationResult();

        if (instance is null)
        {
            result.AddError(string.Empty, ValidationMessages.Get("NullInstance"));

            return result;
        }

        foreach (var rule in _rules)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await rule.ValidateAsync(
                instance,
                result,
                cancellationToken);
        }

        return result;
    }
}
