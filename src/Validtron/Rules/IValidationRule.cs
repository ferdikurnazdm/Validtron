using Validtron.Results;

namespace Validtron.Rules;

internal interface IValidationRule<in T>
{
    bool ContainsAsyncValidator { get; }

    void Validate(T instance, ValidationResult result);

    Task ValidateAsync(
        T instance,
        ValidationResult result,
        CancellationToken cancellationToken);
}