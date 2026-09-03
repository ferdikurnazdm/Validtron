namespace Validtron.Exceptions;

public sealed class UnsupportedEnumUnderlyingTypeException
    : InvalidOperationException
{
    public UnsupportedEnumUnderlyingTypeException(Type underlyingType)
        : base($"Unsupported enum underlying type: {underlyingType}.")
        => UnderlyingType = underlyingType;

    public Type UnderlyingType { get; }
}
