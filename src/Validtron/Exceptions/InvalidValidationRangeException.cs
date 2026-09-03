namespace Validtron.Exceptions;

public sealed class InvalidValidationRangeException : ArgumentException
{
    public InvalidValidationRangeException()
        : base("The minimum value must be less than or equal to the maximum value.")
    {

    }

    public InvalidValidationRangeException(string message)
        : base(message)
    {

    }

    public InvalidValidationRangeException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
