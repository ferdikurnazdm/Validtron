using System.Linq.Expressions;
using Validtron.Configurations;
using Validtron.Exceptions;
using Validtron.Internal;
using Validtron.Results;

namespace Validtron.Rules;

internal sealed class ValidationRule<T, TProperty> : IValidationRule<T>, IConfigurableValidationRule<T, TProperty>
{
    private readonly Func<T, TProperty> _propertySelector;

    private readonly string _propertyName;

    private readonly List<ValidationStep<T, TProperty>> _steps = [];

    private Func<T, bool>? _condition;

    private CascadeMode _cascadeMode = ValidationDefaults.DefaultCascadeMode;



    public ValidationRule(Expression<Func<T, TProperty>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        _propertyName = PropertyPathHelper.GetPropertyPath(expression);

        _propertySelector = expression.Compile();
    }

    public bool ContainsAsyncValidator => _steps.Any(step => step.AsyncPredicate is not null ||
       (step.ChildValidator is not null &&
        AsyncValidationDetector.ContainsAsyncValidator(step.ChildValidator)));


    public void AddStep(ValidationStep<T, TProperty> step)
    {
        ArgumentNullException.ThrowIfNull(step);

        _steps.Add(step);
    }

    public void AddCondition(Func<T, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);

        _condition = CombineWithExisting(condition);
    }

    public void SetCascadeMode(CascadeMode mode) => _cascadeMode = mode;


    private Func<T, bool> CombineWithExisting(Func<T, bool> next)
    {
        var previous = _condition;

        return previous is null
            ? next
            : instance => previous(instance) && next(instance);
    }

    public void Validate(T instance, ValidationResult result)
    {
        if (_condition is not null && !_condition(instance))
        {
            return;
        }

        var value = _propertySelector(instance);

        foreach (var step in _steps)
        {
            var failed = false;

            if (step.ChildValidator is not null)
            {
                if (value is null)
                {
                    continue;
                }

                var childResult = step.ChildValidator.Validate(value);

                MergeChildResult(childResult, result);

                failed = !childResult.IsValid;
            }
            else
            {
                if (step.SyncPredicate is null)
                {
                    throw new AsyncValidationRequiredException();
                }

                var isValid = step.SyncPredicate(instance, value);

                if (!isValid)
                {
                    result.AddError(_propertyName, step.ErrorMessage!);

                    failed = true;
                }
            }

            if (failed && _cascadeMode == CascadeMode.Stop)
            {
                break;
            }
        }
    }

    public async Task ValidateAsync(T instance, ValidationResult result, CancellationToken cancellationToken)
    {
        if (_condition is not null && !_condition(instance))
        {
            return;
        }

        var value = _propertySelector(instance);

        foreach (var step in _steps)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var failed = false;

            if (step.ChildValidator is not null)
            {
                if (value is null)
                {
                    continue;
                }

                var childResult = await step.ChildValidator.ValidateAsync(value, cancellationToken);

                MergeChildResult(childResult, result);

                failed = !childResult.IsValid;
            }
            else
            {
                var isValid = step.AsyncPredicate is not null
                    ? await step.AsyncPredicate(
                        instance,
                        value,
                        cancellationToken)
                    : step.SyncPredicate!(
                        instance,
                        value);

                if (!isValid)
                {
                    result.AddError(_propertyName, step.ErrorMessage!);

                    failed = true;
                }
            }

            if (failed && _cascadeMode == CascadeMode.Stop)
            {
                break;
            }
        }
    }

    private void MergeChildResult(ValidationResult childResult, ValidationResult result)
    {
        foreach (var failure in childResult.Errors)
        {
            result.AddError(PropertyPathHelper.CombineChildPropertyName(
                    _propertyName,
                    failure.PropertyName),
                failure.ErrorMessage);
        }
    }
}