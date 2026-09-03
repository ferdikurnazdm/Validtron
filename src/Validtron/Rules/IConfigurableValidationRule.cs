using Validtron.Configurations;

namespace Validtron.Rules;

internal interface IConfigurableValidationRule<T, TProperty>
{
    void AddStep(ValidationStep<T, TProperty> step);

    void AddCondition(Func<T, bool> condition);

    void SetCascadeMode(CascadeMode mode);
}