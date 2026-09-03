using System.Linq.Expressions;
using Validtron.Configurations;
using Validtron.Exceptions;
using Validtron.Internal;
using Validtron.Results;

namespace Validtron.Rules;

internal sealed class ValidationRuleCollection<T, TProperty> : IValidationRule<T>, IConfigurableValidationRule<T, TProperty>
{
    private readonly Func<T, IEnumerable<TProperty>?> _collectionSelector;

    private readonly string _propertyName;

    private readonly List<ValidationStep<T, TProperty>> _steps = [];

    private Func<T, bool>? _condition;

    private CascadeMode _cascadeMode = ValidationDefaults.DefaultCascadeMode;




    public ValidationRuleCollection(Expression<Func<T, IEnumerable<TProperty>?>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        _collectionSelector = expression.Compile();

        _propertyName = PropertyPathHelper.GetPropertyPath(expression);
    }

    public bool ContainsAsyncValidator => _steps.Any(step => step.AsyncPredicate is not null ||
        (step.ChildValidator is not null && AsyncValidationDetector.ContainsAsyncValidator(step.ChildValidator)));



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

        var collection = _collectionSelector(instance);

        if (collection is null)
        {
            return;
        }

        var index = 0;

        foreach (var element in collection)
        {
            var elementPropertyName = $"{_propertyName}[{index}]";

            foreach (var step in _steps)
            {
                var failed = false;

                if (step.ChildValidator is not null)
                {
                    if (element is null)
                    {
                        continue;
                    }

                    var childResult = step.ChildValidator.Validate(element);

                    MergeChildResult(elementPropertyName, childResult, result);

                    failed = !childResult.IsValid;
                }
                else
                {
                    if (step.SyncPredicate is null)
                    {
                        throw new AsyncValidationRequiredException();
                    }

                    var isValid = step.SyncPredicate(instance, element);

                    if (!isValid)
                    {
                        result.AddError(elementPropertyName, step.ErrorMessage!);

                        failed = true;
                    }
                }

                if (failed && _cascadeMode == CascadeMode.Stop)
                {
                    break;
                }
            }

            index++;
        }
    }

    public async Task ValidateAsync(T instance, ValidationResult result, CancellationToken cancellationToken)
    {
        if (_condition is not null && !_condition(instance))
        {
            return;
        }

        var collection = _collectionSelector(instance);

        if (collection is null)
        {
            return;
        }

        var index = 0;

        foreach (var element in collection)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var elementPropertyName = $"{_propertyName}[{index}]";

            foreach (var step in _steps)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var failed = false;

                if (step.ChildValidator is not null)
                {
                    if (element is null)
                    {
                        continue;
                    }

                    var childResult = await step.ChildValidator.ValidateAsync(element, cancellationToken);

                    MergeChildResult(elementPropertyName, childResult, result);

                    failed = !childResult.IsValid;
                }
                else
                {
                    var isValid = step.AsyncPredicate is not null
                        ? await step.AsyncPredicate(
                            instance,
                            element,
                            cancellationToken)
                        : step.SyncPredicate!(
                            instance,
                            element);

                    if (!isValid)
                    {
                        result.AddError(elementPropertyName, step.ErrorMessage!);

                        failed = true;
                    }
                }

                if (failed && _cascadeMode == CascadeMode.Stop)
                {
                    break;
                }
            }

            index++;
        }
    }

    private static void MergeChildResult(string elementPropertyName, ValidationResult childResult, ValidationResult result)
    {
        foreach (var failure in childResult.Errors)
        {
            result.AddError(PropertyPathHelper.CombineChildPropertyName(
                    elementPropertyName,
                    failure.PropertyName),
                failure.ErrorMessage);
        }
    }
}
