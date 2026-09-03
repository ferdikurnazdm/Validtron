namespace Validtron.Results;

public sealed record ValidationFailure(
    string PropertyName,
    string ErrorMessage);
