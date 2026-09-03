namespace Validtron.Exceptions;

public sealed class InvalidValidationLengthRangeException : ArgumentException
{
    public InvalidValidationLengthRangeException()
        : base("The minimum length must be less than or equal to the maximum length.")
    {

    }

    public InvalidValidationLengthRangeException(string message)
        : base(message)
    {

    }

    public InvalidValidationLengthRangeException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
