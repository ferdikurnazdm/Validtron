namespace Validtron.Internal;

internal interface IAsyncValidationMetadata
{
    bool ContainsAsyncValidator { get; }
}

internal static class AsyncValidationDetector
{
    public static bool ContainsAsyncValidator<T>(IValidator<T> validator)
    {
        ArgumentNullException.ThrowIfNull(validator);

        return validator is not IAsyncValidationMetadata metadata || metadata.ContainsAsyncValidator;
    }
}