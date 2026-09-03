namespace Validtron.Rules;

internal sealed record ValidationStep<T, TProperty>
{
    public Func<T, TProperty, bool>? SyncPredicate { get; }

    public Func<T, TProperty, CancellationToken, Task<bool>>? AsyncPredicate { get; }

    public IValidator<TProperty>? ChildValidator { get; }

    public string? ErrorMessage { get; }

    private ValidationStep(
        Func<T, TProperty, bool>? syncPredicate,
        Func<T, TProperty, CancellationToken, Task<bool>>? asyncPredicate,
        IValidator<TProperty>? childValidator,
        string? errorMessage)
    {
        SyncPredicate = syncPredicate;
        AsyncPredicate = asyncPredicate;
        ChildValidator = childValidator;
        ErrorMessage = errorMessage;
    }

    internal static ValidationStep<T, TProperty> Sync(
        Func<T, TProperty, bool> predicate,
        string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

        return new ValidationStep<T, TProperty>(
            predicate,
            null,
            null,
            errorMessage);
    }

    internal static ValidationStep<T, TProperty> Async(
        Func<T, TProperty, CancellationToken, Task<bool>> predicate,
        string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

        return new ValidationStep<T, TProperty>(
            null,
            predicate,
            null,
            errorMessage);
    }

    internal static ValidationStep<T, TProperty> Child(
        IValidator<TProperty> validator)
    {
        ArgumentNullException.ThrowIfNull(validator);

        return new ValidationStep<T, TProperty>(
            null,
            null,
            validator,
            null);
    }
}
