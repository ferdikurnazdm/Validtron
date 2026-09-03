namespace Validtron.Exceptions;

public sealed class AsyncValidationRequiredException : InvalidOperationException
{
    public AsyncValidationRequiredException()
        : base(
            "This validator contains asynchronous rules. " +
            "Use ValidateAsync instead of Validate.")
    {

    }

    public AsyncValidationRequiredException(string message)
        : base(message)
    {

    }

    public AsyncValidationRequiredException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {

    }
}
