namespace Validtron.Results;

public sealed class ValidationResult
{
    private readonly List<ValidationFailure> _errors = [];

    private readonly HashSet<(string PropertyName, string ErrorMessage)> _errorKeys =
        [];

    public bool IsValid => _errors.Count == 0;

    public IReadOnlyList<ValidationFailure> Errors => _errors;

    public IReadOnlyDictionary<string, IReadOnlyList<string>> ErrorsByProperty =>
        _errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<string>)group
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToList());

    internal void AddError(string propertyName, string errorMessage)
    {
        if (!_errorKeys.Add((propertyName, errorMessage))) return;

        _errors.Add(
            new ValidationFailure(
                propertyName,
                errorMessage));
    }
}
